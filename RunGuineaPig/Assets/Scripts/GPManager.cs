using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GPManager : MonoBehaviour
{
    
    //public Transform GuineaPig;
    public Transform GuineaPig;

    private Vector3 position = new Vector3(0.01719905f, 0.67f, -1.113391f);
    public GameObject[] guineaPigs = new GameObject[6];

    public Vector3[] nextDest = new Vector3[6];

    private List<int[]> groups = new List<int[]>();

    public static GPManager instance;

    public Dictionary<int, Queue<int>> mergeList = new Dictionary<int, Queue<int>>();

    //work need to be done after merge finish
    //public List<int> removeList;


    //Adding destination when merge turn
    //int: leader index
    //vector: destination to turn
    public Dictionary<int, Vector3> leaderTempPos = new Dictionary<int, Vector3>();




     void Awake()
    {
        
        if(instance != null)
        {
            Debug.Log("more than 1 manager");
        }
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        int[] defaultTeam = new int[6];
        //add guinea pig to array
        for(int i = 0; i < 6; i++){
            
            Transform pigTrans = Instantiate(GuineaPig, position, Quaternion.identity);
            GameObject pig = pigTrans.gameObject;

            var GuineaPB = pig.GetComponent<GPBehavior>();
            GuineaPB.index = i;

            //disable front of pigs that are not leader
            if (i > 0){
                
                GuineaPB.DisableFront();
            }
            //set default leader index to 0(first pig);
            GuineaPB.leaderIndex = 0;


            //record all guinea pig
            guineaPigs[i] = pig;
            //add to default group
            defaultTeam[i] = GuineaPB.index;

            position.x -= 1;
            //Instantiate(GuineaPig);
        }

        //add default group
        groups.Add(defaultTeam);

        //removeList = new List<int>();

        
        
        //for(int j = 0; j < 6; j++){
        //    Debug.Log("my tag is " + guineaPigs[j].tag);
        //}
    }

    // Update is called once per frame
    /**
    void Update()
    {
        foreach(var team in groups){
            GameObject leader = team[0];
            var leaderPB = leader.GetComponent<GPBehavior>();
            if (!leaderPB.GetCanMove()){
                for(int m = 1; m < team.Length; m++){
                    //set canmove to false
                    GameObject pig = team[m];
                    var pigPB = pig.GetComponent<GPBehavior>();
                    pigPB.SetCanMove(false);
                }
            }
        }
    }
    */


    void LateUpdate(){
        if(!Global.Start) return;
        foreach(KeyValuePair<int, Queue<int>> dict in mergeList){
            //Debug.Log("key is " + dict.Key);
            Queue<int> valQueue = dict.Value;
            //foreach(int b in valQueue){
            //    Debug.Log("val is " + b);
            //}
            //Debug.Log("================finish================");

        }

        foreach(GameObject gp in guineaPigs){
            
            if (gp != null){
                if(!gp.GetComponent<GPBehavior>().getPriorityStop()){
                    var gpBH = gp.GetComponent<GPBehavior>();
                    gpBH.TrueMove();
                }
            }
        }

        //if (removeList.Count > 0){
        //    foreach(int ind in removeList){
        //        GameObject pig = guineaPigs[ind];
        //        if (pig != null){
        //            var pigComp = pig.GetComponent<GPBehavior>();
        //            pigComp.setPriorityStop(false);
        //        }
                

        //    }
        //    removeList.Clear();
        //}
            
        for(int a = 0; a < groups.Count; a++){
            if(groups[a] != null){
            //for each group
                int[] currentGroup = groups[a];
                //find the leader index
                int leaderIndex = currentGroup[0];
                if (guineaPigs[leaderIndex] != null){

                    Queue<int> mList = new Queue<int>();
                    if (mergeList.ContainsKey(leaderIndex)){
                        mList = mergeList[leaderIndex];
                    }
                    else{
                        mergeList.Add(leaderIndex, mList);
                    }

                    for(int i = 0; i < nextDest.Length; i++){
                        Vector3 pigNextDest = nextDest[i];
                        
                        
                        if(i != leaderIndex &&
                            guineaPigs[i] != null &&
                            Vector3.Distance(pigNextDest, nextDest[leaderIndex]) <= 0.001f){
                                
                                GameObject memPig = guineaPigs[i];
                                var memPigPB = memPig.GetComponent<GPBehavior>();
                                int mergedLeaderIndex = memPigPB.leaderIndex;

                                GameObject mLd = guineaPigs[mergedLeaderIndex];
                                var mLdPB = mLd.GetComponent<GPBehavior>();

                                GameObject lLd = guineaPigs[leaderIndex];
                                var lLdPB = lLd.GetComponent<GPBehavior>();

                                int indexDiff = Math.Abs(lLdPB.getDirIndex() - mLdPB.getDirIndex());

                                //if(i == mergedLeaderIndex && indexDiff  == 2){
                                    //ignore the two leader collide case
                                    
                                //}
                                //else{
                                    if (mergeList.ContainsKey(mergedLeaderIndex)){
                                        Queue<int> mergingTeam = mergeList[mergedLeaderIndex];
                                        int[] testMerge = mergingTeam.ToArray();
                                        Debug.Log("merging team");
                                        foreach(int integer in testMerge){
                                            Debug.Log("I have: " + integer);
                                        }
                                        
                                        if (!mergingTeam.Contains(leaderIndex) && !mList.Contains(mergedLeaderIndex) && mergedLeaderIndex != leaderIndex){
                                            Debug.Log("i is " + i + " leader index is " + leaderIndex);
                                            Debug.Log("2 leader index " + leaderIndex + " is merging with" + memPigPB.leaderIndex);
                                            //case 1
                                            Debug.Log("leader " + leaderIndex + " should contain in " + mergedLeaderIndex + ". Answer is " + mergingTeam.Contains(leaderIndex));
                                            //case 2
                                            
                                            mList.Enqueue(mergedLeaderIndex);
                                            leaderTempPos.Add(leaderIndex, nextDest[leaderIndex]);
                                        }

                                    }
                                    else{
                                        if (!mList.Contains(mergedLeaderIndex)){
                                            Debug.Log("leader index " + leaderIndex + " is merging with" + memPigPB.leaderIndex);
                                            mList.Enqueue(mergedLeaderIndex);
                                            Debug.Log("I have stored data in dict: " + mergeList[leaderIndex].Count);
                                            leaderTempPos.Add(leaderIndex, nextDest[leaderIndex]);
                                        }
                                    }
                                //}

                        }
                    }

                    

                    if(mList.Count > 0){
                        int mergedLeader = mList.Peek();
                        List<int> mergedGroup = new List<int>();

                        
                        

                        
                        for(int j = 0; j < groups.Count; j++){
                            if (groups[j] != null){
                                int[] g = groups[j];
                                if (g[0] == mergedLeader){
                                    for (int i = 0; i < g.Length; i++){
                                        //index 
                                        Debug.Log("g[i] is " + g[i]);
                                        mergedGroup.Add(g[i]);
                                    }
                                    //groups[j] = null;

                                }
                                //set the original merged group to null

                                   
                            }
                            
                            
                        }

                        //Debug.Log("removed is " + removed);
                        

                        // bug 预警 三头猪 length = 0
                        Debug.Log("length of merged group is " + mergedGroup.Count);
                        int lastMemIndex = mergedGroup[mergedGroup.Count - 1];
                        GameObject lastMem = guineaPigs[lastMemIndex];
                        var lastMemPB = lastMem.GetComponent<GPBehavior>();

                        Debug.Log("3's next dest is " + nextDest[2] + " 2's next dest is " + nextDest[3]);
                            
                        if (Vector3.Distance(lastMemPB.getPrevDest(), nextDest[leaderIndex]) <= 0.001f){
                            //merge start
                            Debug.Log("Merge start");
                            
                            //find the leader pig
                            GameObject l = guineaPigs[leaderIndex];
                            var lPigB = l.GetComponent<GPBehavior>();
                            int ldDirIndex = lPigB.getDirIndex();
                            int lastDirIndex = lastMemPB.getDirIndex();
                            bool turnRight = false;
                            if ((ldDirIndex - lastDirIndex == 1) || (ldDirIndex == 1 && lastDirIndex == 4)){
                                //turn left
                                turnRight = false;
                            }
                            else if ((ldDirIndex - lastDirIndex == -1) || (ldDirIndex == 4 && lastDirIndex == 1)){
                                //turn right
                                turnRight = true;
                            }



                            
                            foreach(int inde in currentGroup){
                                GameObject pigObj = guineaPigs[inde];
                                var pigObjPB = pigObj.GetComponent<GPBehavior>();

                                Debug.Log("I am id: " + inde);
                                //Debug.Log(pigObjPB.turnDir.Count);
                                //Debug.Log(pigObjPB.turnPos.Count);

                                

                                pigObjPB.turnDir.Enqueue(turnRight);

                                Vector3 pos = leaderTempPos[leaderIndex];
                                pigObjPB.turnPos.Enqueue(pos);

                                //test
                                bool[] testDir = pigObjPB.turnDir.ToArray();
                                Vector3[] testPos = pigObjPB.turnPos.ToArray();

                                for(int z = 0; z < testDir.Length; z++){
                                    Debug.Log("DIR is " + testDir[z] + "POS is " + testPos[z]);
                                }
                                
                                //pigObjPB.setSecretMergeIndex(lastMemPB.getDirIndex());
                                //pigObjPB.setSecretDestination();
                                //pigObjPB.setPriorityTurn(true);
                                pigObjPB.setPriorityStop(false);

                                

                                
                                
                                //finish merge in here but move this to the beginning
                                

                                
                            }

                            //reset data structure
                            //currentGroup and mergedGroup

                            int newLen = currentGroup.Length + mergedGroup.Count;
                            int[] newGroup = new int[newLen];

                            for(int n = 0; n < newGroup.Length; n++){
                                if (n < mergedGroup.Count){
                                    newGroup[n] = mergedGroup[n];
                                }
                                else{
                                    newGroup[n] = currentGroup[n - mergedGroup.Count];
                                }

                            }

                            //change the ds
                            for(int pIndex = 0; pIndex < newGroup.Length; pIndex++){
                                GameObject teamPig = guineaPigs[newGroup[pIndex]];
                                var teamPigCmp = teamPig.GetComponent<GPBehavior>();

                                teamPigCmp.leaderIndex = newGroup[0];

                                if(pIndex != 0){
                                    teamPigCmp.DisableFront();
                                }
                                else{
                                    //double check
                                    teamPigCmp.ableFront();
                                }
                            }    

                            //test
                            Debug.Log("new group contains");
                            foreach(int index1 in newGroup){
                                GameObject pg = guineaPigs[index1];
                                var pgBH = pg.GetComponent<GPBehavior>();
                                Debug.Log(index1 + " my leader is " + pgBH.leaderIndex);
                            }
                            
                            


                            Debug.Log("Merge Finish");
                            mList.Dequeue();
                            Debug.Log("after merge finish");
                            Debug.Log("this mlist belongs to " + leaderIndex);
                            int[] testArr2 = mList.ToArray();
                            foreach(int mInt in testArr2){
                                Debug.Log(mInt);
                            }


                            //next, handle with mergeList
                            if (mergeList.ContainsKey(mergedLeader)){
                                Queue<int> newMergedList = mergeList[mergedLeader];
                                //directly add
                                foreach(int integ in mList){
                                    //if new list does not contain this merge group leader index
                                    if (!newMergedList.Contains(integ)){
                                        newMergedList.Enqueue(integ);
                                    }
                                }

                            }
                            else{
                                //create a new one
                            }

                            for(int c = 0; c < groups.Count; c++){
                                int[] cGroup = groups[c];
                                if ( cGroup[0] == mergedLeader){
                                    groups[c] = null;
                                    break;
                                }
                            }
                                
                            groups.RemoveAt(a);
                            groups.Insert(a, newGroup);


                            //handle with leader temp pos
                            leaderTempPos.Remove(leaderIndex);

                            Debug.Log("================FINISH===============");

                            //remove mList
                            mergeList.Remove(leaderIndex);

                            //test
                            foreach(KeyValuePair<int, Queue<int>> dict in mergeList){
                                Debug.Log("key is " + dict.Key);
                                Queue<int> valQueue = dict.Value;
                                foreach(int b in valQueue){
                                    Debug.Log("val is " + b);
                                }
                                Debug.Log("================finish================");
                            }
                            
                            
                        }
                        else{
                            Debug.Log("not mergining");
                            foreach(int inde in currentGroup){
                                GameObject pigObj = guineaPigs[inde];
                                var pigObjPB = pigObj.GetComponent<GPBehavior>();

                                if(!pigObjPB.getPriorityStop()){
                                    pigObjPB.setPriorityStop(true);
                                }
                            }

                            foreach(int[] myGroup in groups){
                                if(myGroup != null){
                                Debug.Log("my group leader is " + myGroup[0]);
                                foreach(int mem in myGroup){
                                    Debug.Log("member " + mem);
                                    }   
                                }
                            }
                            Debug.Log("=============in not merging===============");

                        }

                        

                    }
                    

        

                    
                }
            }

            //groups.RemoveAt(a);
        }

        foreach(int[] myGroup in groups){
            if(myGroup != null){
                Debug.Log("my group leader is " + myGroup[0]);
                foreach(int mem in myGroup){
                    Debug.Log("member " + mem);
                }
            }
        }
        Debug.Log("=============before round===============");


        List<int> waitToRemove = new List<int>();
        for(int count = 0; count < groups.Count; count++){
            if (groups[count] == null){
                waitToRemove.Add(count);

            }
        }

        for(int count = waitToRemove.Count - 1; count >= 0; count--){
            groups.RemoveAt(waitToRemove[count]);
        }
            

        foreach(int[] myGroup in groups){
            if(myGroup != null){
                Debug.Log("my group leader is " + myGroup[0]);
                foreach(int mem in myGroup){
                    Debug.Log("member " + mem);
                }
            }
        }
        Debug.Log("=============round finish===============");

        
    }



    public bool sendStopSignal(int index){
        foreach(int[] group in groups){
            //find the team
            if (group[0] == index){
                //send stop msg to all team member
                for(int i = 1; i < group.Length; i++){
                    GameObject pig = guineaPigs[group[i]];
                    var GuineaPB = pig.GetComponent<GPBehavior>();
                    GuineaPB.SetCanMove(false);
            
                }
                return true;
            }
        }
        Debug.Log("cannot find the team");
        return false;
        
    } 

    public bool sendContinueSignal(int index){
        foreach(int[] group in groups){
            //find the team
            if(group != null){
                if (group[0] == index){
                    //send stop msg to all team member
                    for(int i = 1; i < group.Length; i++){
                        if(guineaPigs[group[i]] != null){
                            GameObject pig = guineaPigs[group[i]];
                            var GuineaPB = pig.GetComponent<GPBehavior>();
                            GuineaPB.SetCanMove(true);
                        }
                
                    }
                    return true;
                }
            }
        }

        Debug.Log("cannot find the team");
        return false;

    }

    public bool sendTurnSignal(int index, bool direction, Vector3 turnPos){
        foreach(int[] group in groups){
            if (group[0] == index){
                for(int i = 1; i < group.Length; i++){
                    GameObject pig = guineaPigs[group[i]];
                    var GuineaPB = pig.GetComponent<GPBehavior>();
                    GuineaPB.turnPos.Enqueue(turnPos);
                    GuineaPB.turnDir.Enqueue(direction);
                    //Debug.Log("index: " + GuineaPB.index + ", turnPos: " + GuineaPB.turnPos.Peek() + " is added");
                    
                }
                return true;
            }

        }
        return false;

    }

    public bool sendSplitTeam(int oldLeaderIndex, int newLeaderIndex){
        Debug.Log("===========Spliting Team==============");
        for(int i = 0; i < groups.Count; i++){
            int[] currentGroup = groups[i];
            //Debug.Log("current group is " + i);
            
            if(currentGroup[0] == oldLeaderIndex){
                //find the old leader
                //split team
                int lenA = -1;
                int lenB = -1;
                for(int j = 0; j < currentGroup.Length; j++){
                    if (currentGroup[j] == newLeaderIndex){
                        lenA = j;
                        lenB = currentGroup.Length - lenA;
                        //Debug.Log("len of A is " + lenA + ", len of B is " + lenB);
                    }
                }
                if(lenA < 1 || lenB < 1){
                    return false;
                }
                //first group
                
                int[] groupA = new int[lenA];
                Array.Copy(currentGroup, groupA,lenA);

                //second group
                int[] groupB = new int[lenB];
                Array.Copy(currentGroup, lenA, groupB, 0, lenB);
                GameObject leaderPig = guineaPigs[groupB[0]];
                var GuineaPB = leaderPig.GetComponent<GPBehavior>();
                GuineaPB.ableFront();
                Vector3 gTurnPos = GuineaPB.lastPos;
                bool gTurnDir = GuineaPB.lastDir;
                //Debug.Log("head is " + GuineaPB.index);
                //Debug.Log("lastPos is " + gTurnPos + ", lastDir is " + gTurnDir);

                
                
                //clear all the step that leader discard and add the rest to new queue
                foreach(int index in groupB){
                    GameObject memberPig = guineaPigs[index];
                    var GuineaPB1 = memberPig.GetComponent<GPBehavior>();
                    GuineaPB1.leaderIndex = groupB[0];


                    //Debug.Log("I'm " + index + ", my leader is " + GuineaPB1.leaderIndex);
                    //GuineaPB1.turnPos.Clear();
                    //GuineaPB1.turnDir.Clear();
                    //Debug.Log("I'm gonna print everything in queue");
                    

                    
                    if (index != groupB[0]){
                        Queue<Vector3> gPos = GuineaPB1.turnPos;
                        Queue<bool> gDir = GuineaPB1.turnDir;
                        //Debug.Log("gPos has length: " + gPos.Count + ", gDir has length: " + gDir.Count);
                        //Debug.Log("gPos first element is " + gPos.Peek() + ", gDir first element is " + gDir.Peek());

                        
                        Queue<Vector3> tempPos = new Queue<Vector3>();
                        Queue<bool> tempDir = new Queue<bool>();

                        if(gPos.Contains(gTurnPos)){
                            while(gPos.Count > 0 && gDir.Count > 0 ){
                                //Debug.Log("gPos is  " + gPos.Peek() + ", gTurnPos is " + gTurnPos);
                                if(Vector3.Distance(gPos.Peek(), gTurnPos) <= 0.0001f){
                                    tempPos.Enqueue(gPos.Dequeue());
                                    tempDir.Enqueue(gDir.Dequeue());
                                    //Debug.Log("pos is " + tempPos.Peek() + ", dir is " + tempDir.Peek());
                                    //Debug.Log("clearing");
                                    gPos.Clear();
                                    gDir.Clear();
                                }
                                else{
                                    tempPos.Enqueue(gPos.Dequeue());
                                    tempDir.Enqueue(gDir.Dequeue());
                                }
                                
                                //Debug.Log("count is " + gPos.Count + " now");
                            }

                            while(tempPos.Count > 0){
                                gPos.Enqueue(tempPos.Dequeue());
                                gDir.Enqueue(tempDir.Dequeue());
                            }


                        }
                        else{
                            //already follow the last step of leader
                            gPos.Clear();
                            gDir.Clear();
                        }

                    }
                    else{
                        GuineaPB1.turnPos.Clear();
                        GuineaPB1.turnDir.Clear();
                    }
                    
                    
                }
                

                groups.RemoveAt(i);
                groups.Add(groupA);
                groups.Add(groupB);

                return true;

            }
            
        }
        return false;
        
    }


    public bool sendDyingMsg(int pigIndex, int leaderIndex){
        
        
        //find the team of dead pig
        for(int n = 0; n < groups.Count; n++){
            int[] group = groups[n];
            if (group[0] == leaderIndex){
                if (group.Length > 1){
                    //more than 1 member in team
                    int[] newGroup = new int[group.Length - 1];
                    //Debug.Log("My original length is " + group.Length);
                    //Debug.Log("I have length " + newGroup.Length);
                    //Debug.Log("My Leader is " + group[0]);
                    //Debug.Log(pigIndex + " is dead");
                    if (pigIndex == leaderIndex){

                        // leader dead, change leader to 2nd pig
                        Debug.Log("leader: " + pigIndex + " die");
                        for(int i = 1; i < group.Length; i++){
                            GameObject pig = guineaPigs[group[i]];
                            var GuineaPB = pig.GetComponent<GPBehavior>();
                            GuineaPB.leaderIndex = group[1];
                            Debug.Log("new leader will be " + GuineaPB.leaderIndex);

                            if (i == 1){
                                GuineaPB.ableFront();
                            }
                        }

                        //change the data structure related when handling merge
                        Debug.Log("preparing to modify mergeList...");
                        Debug.Log("leader index is " + leaderIndex);
                        if (mergeList.ContainsKey(leaderIndex)){
                            Debug.Log("merge list contains key with leader index");
                            Queue<int> leaderMerge = mergeList[leaderIndex];
                            mergeList.Add(group[1], leaderMerge);
                            mergeList.Remove(leaderIndex);
                                
                        }

                        foreach(KeyValuePair<int, Queue<int>> dict in mergeList){
                            Queue<int> mergQ = dict.Value;
                            int mergIndex = dict.Key;
                            Debug.Log("original merge q contains: ");
                            foreach(int a in mergQ){
                                Debug.Log(a);
                            }
                            if (mergQ.Contains(leaderIndex)){
                                //if leader index is in any queue, remove it
                                Queue<int> newMergQ = new Queue<int>();

                                for(int j = 0; j < mergQ.Count; j++){  
                                    if(mergQ.Peek() != leaderIndex){
                                        newMergQ.Enqueue(mergQ.Dequeue());
                                    }
                                    else{
                                        newMergQ.Enqueue(group[1]);
                                        mergQ.Dequeue();
                                    }
                                }
                                
                                mergeList[mergIndex] = newMergQ;
                                Debug.Log("mergIndex: " + mergIndex);
                                foreach(int a in newMergQ){
                                    Debug.Log(a);
                                }

                            }
                        }
                            


                    }

                    //create new group which remove dead pig
                    int ind = 0;
                    for(int j = 0; j < group.Length; j++){
                        //Debug.Log("new group length " + newGroup.Length);
                        if (group[j] != pigIndex){
                            newGroup[ind] = group[j];
                            //Debug.Log("new is " + ind + ": " + newGroup[ind]);
                            ind++;
                        }

                    }

                    groups.Add(newGroup);
                    groups.RemoveAt(n);

                    //test

                    //foreach(int[] pigGroup in groups){
                    //    Debug.Log("group: ");
                    //    Debug.Log(pigGroup.Length);
                    //    foreach(int m in pigGroup){
                    //        Debug.Log("I'm " + m);
                    //    }
                    //}
                }
                else if (group.Length == 1){
                    //last member dead
                    groups.Remove(group);

                }

                //delete info from guineaPig array
                guineaPigs[pigIndex] = null;
                //nextDest[pigIndex] = null;

                return true;
            }

        }
        Debug.Log("Cannot find the team");
        return false;

    }

}
