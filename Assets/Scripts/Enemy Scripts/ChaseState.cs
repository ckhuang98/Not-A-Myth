using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
//using Tuple;

public class ChaseState : BaseState
{
    private Enemy _enemy;
    //This is the player
    private Transform target;
    public float speed = 3f;
    public float turnTimer = 0f;
    private float angle;
    private GameObject[] hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
    private GameObject[] fireImps = GameObject.FindGameObjectsWithTag("Fire Imp");
    private GameObject[] fireEels = GameObject.FindGameObjectsWithTag("Fire Eel");
    private GameObject[] swordGiants = GameObject.FindGameObjectsWithTag("Sword Giant");
    private GameObject walls = GameObject.Find("Walls");
    private bool choice;

    //private (int, int)[] successors = new NaN

    /*
    Purpose: constructor recieves all needed values from enemy class and recieves
    the transform component from the player.
    Recieves: the enemy class from the enemy.cs file
    Returns: nothing
    */
    public ChaseState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    /*
    Purpose: Calls all functions in order to successfully chase the player
    Recieves: nothing
    Returns: the type of the current chase state consistently returned, until the 
    enemy gets close, then the type of the attack state is returned
    */
    public override Type Tick()
    { 
        if (_enemy.beenHit == false && _enemy.tag == "Hammer Giant") {
            _enemy.enemyAnimator.SetFloat("Horizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
            _enemy.enemyAnimator.SetFloat("Vertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
            speed = 1f;
        } else if (_enemy.beenHit == true && _enemy.tag == "Hammer Giant") {
            _enemy.enemyAnimator.SetFloat("HammerHitHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
            _enemy.enemyAnimator.SetFloat("HammerHitVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
            speed = .25f;
        }
        _enemy.inBounds = false;
        transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * speed * Time.deltaTime;

        if (_enemy.tag == "Sword Giant") {
            _enemy.enemyAnimator.SetFloat("SwordWalkHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
            _enemy.enemyAnimator.SetFloat("SwordWalkVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
        }

        //Debug.DrawRay(transform.position, _enemy.moveDirections[_enemy.currMoveDirection] * 3.0f, Color.blue);
        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        _enemy.attackAngle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        if (angle < 0.0f ) {
            angle = angle + 360f;
            _enemy.attackAngle = _enemy.attackAngle + 360f;
        } 
        LocatePlayer(angle); 
        WallDetection();
        FailSafeDirection();  
        MoveDirection();   
        NPCDetection();

        if (Vector3.Distance(transform.position, walls.transform.position) < 2.0f) {
            Vector3 dist = transform.position - walls.transform.position;
            transform.position += dist * Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, target.position) <= 2 && _enemy.tag == "Sword Giant") {
            _enemy.enemyAnimator.SetTrigger("SwordAttacking");
            return typeof(SwordAttackState);
        }
        if (Vector2.Distance(transform.position, target.position) <= 2 && _enemy.tag == "Hammer Giant" && _enemy.beenHit == false) {
            _enemy.enemyAnimator.SetTrigger("Attack");
            return typeof(AttackState);
        } else if (Vector2.Distance(transform.position, target.position) >= 20 ) {
            Debug.Log("Moving to wander state");
            return typeof(WanderState);
        } 

        return typeof(ChaseState);
    }

    /*
    Purpose: Uses the angle of the player relative to the enemy, and picks the proper
    move direction accordingly.
    Recieves: A float "angle" which is the angle relative to the player and the enemy.
    Returns: nothing
    */
    private void LocatePlayer(float angle) {
        // transform.position += _enemy.moveDirections[i] * speed * Time.deltaTime;

        // UP
        if (247.5 < angle && angle < 292.5) {  
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[0] = 1;
 
        }
        // RIGHT & UP
        if (202.5 < angle && angle < 247.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[1] = 1;
        }
        // RIGHT
        if (157.5 < angle && angle < 202.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[2] = 1;
    
        }
        // DOWN RIGHT
        if (angle > 112.5 && angle < 157.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[3] = 1;
  
        }
        // DOWN
        if (angle > 67.5 && angle < 112.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
                _enemy.weightList[4] = 1; 
        }
        //DOWN LEFT
        if (angle > 22.5 && angle < 67.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
           _enemy.weightList[5] = 1;
        }
        // LEFT
        if ((angle > 337.5 && angle < 360) || (angle > 0 && angle < 22.5)) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[6] = 1;
        }
        // LEFT & UP
        if (292.5 < angle && angle < 337.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[7] = 1;
        }
    }
    
    /*
    Purpose: If a wall is detected within 1.5 pixels away, the enemy will make a
    180 and walk away from wall.
    Recieves: nothing
    Returns: nothing
    */
    private void WallDetection() {
        // Adjust weight list: -1 for wall, 0 for non-wall
        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            //_enemy.weightList[i] = 0;
            if (_enemy.castList[i].collider != null) {
                if (_enemy.castList[i].distance <= 1.5) {  
                    _enemy.weightList[i] = -1;
                } else {
                    _enemy.weightList[i] = 0;
                }
            }
        }
    }

    /*
    private void findNextDirection() {
        Debug.Log("Stuck Here");
        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            if (_enemy.weightList[i] == 0) {
                _enemy.weightList[i] = 1;
                return;
            }
        }
    }
    */

    /*
    Purpose: If the current move direction finds an obstacle and becomes a weight of
    -1. FialSafeDirection finds the next best direction to take.
    Recieves: nothing
    Returns: nothing
    */
    private void FailSafeDirection() {
        if (_enemy.weightList[_enemy.currMoveDirection] == -1) {
            //If the array is at the end or beginning the choice will be made for them
            if (_enemy.currMoveDirection == 7) {
                choice = false;
            } else if (_enemy.currMoveDirection == 0) {
                choice = true;
            } else {
                choice = (UnityEngine.Random.value > 0.5f);
            }
            if (choice == true) {
                for (int i = _enemy.currMoveDirection; i < _enemy.moveDirections.Count(); i++) {
                    if (_enemy.weightList[i] == 0) {
                        _enemy.weightList[_enemy.currMoveDirection] = 0;
                        _enemy.weightList[i] = 1;
                        break;
                    }
                }
            } else if (choice == false) {
                for (int i = _enemy.currMoveDirection; i >= 0; i--) {
                    if (_enemy.weightList[i] == 0) {
                        _enemy.weightList[_enemy.currMoveDirection] = 0;
                        _enemy.weightList[i] = 1;
                        break;
                    }
                }
            }
        }
    }
            
    /*
    Purpose: sets the current move direction to the direction with a weight of 1
    Recieves: nothing
    Returns: nothign
    */
    private void MoveDirection() {
        turnTimer += Time.deltaTime;
        for (int i = 0; i < _enemy.moveDirections.Count(); i++) {
            //Debug.Log("in da mf loop");
            if (_enemy.weightList[i] == 1) {
                
                if (turnTimer >= 0.5f) {
                    _enemy.currMoveDirection = i;
                    turnTimer = 0f;
                }

            }
        }
    }

    /*
    Purpose: If another enemy if detected they will slowly avoid each other. Different
    distances are based on the enemies different sizes.
    Recieves: nothing.
    Returns: nothing
    */
    private void NPCDetection() {
        foreach (GameObject _hammerGiant in hammerGiants) {
            if (_hammerGiant != null) {
                float currentDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                if (currentDistance < 2.0f)
                {
                Vector3 dist = transform.position - _hammerGiant.transform.position;
                transform.position += dist * Time.deltaTime;
                } 
            }
        }

        foreach (GameObject _fireImp in fireImps) {
            if (_fireImp != null) {
                float currentDistance = Vector3.Distance(transform.position, _fireImp.transform.position);
                if (currentDistance < 2.0f)
                {
                Vector3 dist = transform.position - _fireImp.transform.position;
                transform.position += dist * Time.deltaTime;
                } 
            }
        }

        foreach(GameObject _fireEel in fireEels) {
            if (_fireEel != null) {
                float currentDistance = Vector3.Distance(transform.position, _fireEel.transform.position);
                if (currentDistance < 3.0f) {
                    Vector3 dist = transform.position - _fireEel.transform.position;
                    transform.position += dist *Time.deltaTime;
                }
            }
        }

        foreach(GameObject _swordGiant in swordGiants) {
            if (_swordGiant != null) {
                float currentDistance = Vector3.Distance(transform.position, _swordGiant.transform.position);
                if (currentDistance < 3.0f) {
                    Vector3 dist = transform.position - _swordGiant.transform.position;
                    transform.position += dist *Time.deltaTime;
                }
            }
        }
    }
    /*
    private void MoveDirection() {

    }
    */
    /*
    private void getLowestTrueDistance() {
        // For every 0 in weight list, find 'true cost' with BFS
        // string[] min_path = new string[];
        for (int i = 0; i < _enemy.weightList.Count(); i ++) {
            if (weightList[i] == 0) {
                // path = BFS(curr_state, goal_state)
                // if (min_path != [] and path.Count() < min_path.Count()) {
                    // min_path = path
                ['up right', 'up right', 'up', 'left', 'left', ...]
            }
        }
    }
    */

    /*
    Class Path: will be used to organize searching
    Members:
    | State -   of the form (x,y) where x&y are integers

    | Dirs -    a list of strings, composed of a sequence of directions
                would lead from start state to the goal state

    | Depth -   an integer representing the number of actions from
                start state to current state
    
    public class Path
    {
        public Path()
        {
            var State = null;
            var Dirs = {};
            var Depth = 0;
        }

        public Path((int, int) state, string[] dirs, int depth)
        {
            (int, int) State = state;
            string[] Dirs = dirs;
            int Depth = depth;
        }

        public void set_vars((int, int) state, string dir, int depth)
        {
            State = state;
            Dirs.append(dir);
            Depth += depth;
        }
    }


    /*
    The "managing function" for breadth-first-search, which manages:
    | Paths: A list of Path objects, representing searching paths

    | Fringe: A list of states (x,y) which will be explored

    | Node: the current node to be expanded

    Calls expand, which will explore Node and update Paths to represent
    the changes in the searching
    
    private string[] BFS((int, int) start, (int, int) goal) {
        // paths will be a collection of all currently-exploring paths
        Path[] paths = { };

        var[] fringe = { Tuple.Create(start.Item1, start.Item2) };
        (int, int)[] fringe = { (start[0], start[1]) };

        while (fringe.Count() != 0) {
            (int, int) node = fringe.removeFirst();
            if (node == goal) {
                foreach (Path path in paths) {
                    if (path.state == goal) {
                        return path.dirs;
                    }
                }
            } else {
                expand(state, paths, fringe)
            }
        }
    }
    
    private string[] expand((int, int) state, Path[] paths, (int, int)[] fringe) {

        ((int, int), string, int) successors = successorStates(state);
        remove_higher_depths(successors)


        
        if (paths == {}) {
            foreach ((int, int) node in successors) {
                ((int, int) nextState, dir, cost) = node;
                fringe.append(nextState);
                paths.append(new Path(nextState, [dir], cost));
            }
        }
        else if (successors.Count() == 0) {
            remove_paths(paths, state)
        }
        else if (successors.Count() >= 1) {
            Path dupe = Path();
            ((int, int) nextState, dir, cost) = successors[0];
            fringe.append(nextState);

            for each (Path path in paths) {
                if (state == path.State) {
                    if (successors.Count() >= 2) {
                        dupe = new Path(path.State, path.Dirs, path.Cost);
                    }
                    path.set_vars(nextState, dir, cost);
                }
            }

            if (successors.Count >= 2) {
                for each (Path path in successors) {
                    fringe.append(path.State);
                    paths.append(Path(dupe.State, dupe.Dirs, dupe.Depth))
                    paths[-1].set_vars(path.State, path.Dirs, path.Depth)
                }
            }
            
        }
    }




    private void get_next_state((int, int) currState, int index) {
        switch index {
            case 0 {    // up
                return (currState[0], currState[1] + 1);
            } case 1 {  // up right
                return (currState[0] + 1, currState[1] + 1);
            } case 2 {  // right
                return (currState[0] + 1, currState[1]);
            } case 3 { // down right
                return (currState[0] + 1, currState[1] - 1);
            } case 4 { // down
                return (currState[0], currState[1] - 1);
            } case 5 { // down left
                return (currState[0] - 1, currState[1] - 1);
            } case 6 { // left
                return (currState[0] - 1, currState[1]);
            } case 7 { // up left
                return (currState[0] - 1, currState[1] + 1);
            } default {
                return currState;
            }
        }
    }

    /*
    Returns a list of paths which are not currently obstructed by any obstacles
    (Weight that is 0)
    
    private void successorStates((int, int) state) {
        //A successor state should return (new state, dir. from state->new state, cost from state->new state)
        Path[] successors = {};
        for (int i = 0; i < _enemy.weightList.Count(); i ++) {
            Path succ = Path();
            if (_enemy.weightList[i] == 0) {
                nextState = get_next_state(state, i)
                succ.set_vars()
            }
        }
    }
        

    



                /*
                    // start state: enemy current position
                    curr_state = _enemy.transform.position;

                    // goal state: player position (within x units or so)
                    goal_state = target.transform.position;

                    // successor states: (x,y) coordinates that are not walls

                    ['left', 'left up', 'up'] = get_valid_moves(curr_state);
                    _enemy.weightList[i] == -1;


                    1    2    3    4    5    6
                10  ||| |||  |||  |||  |||  |||
                9   ||| |||   9    9    P   |||     
                8   ||| |||   8    8    8   |||      
                7   ||| |||   7    7    8   |||
                6   ||| |||   6   |||  |||  |||
                5   ||| |||   5    4    4   |||
                4   ||| |||  |||  |||   3   |||      Successors of original state (4,2):
                3   |||  3    2    2    2   |||      { [ (4,3), 'U', 1 ], [ (5,3), 'UR', 1 ], [ (5,2), 'R', 1 ] }
                2   ||| |||   2    1    1   |||      
                1   ||| |||   2    1    E   |||      
                0   ||| |||  |||  |||  |||  |||      

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                fringe = [ (4,2) ]
                | paths:
                | | [ (4,2), {}, 0 ]

                fringe = [ (4,2) ]    ->   expanding (4,2)   ->   found 8 options   ->   duplicate (4,2) path 7 times
                | paths:
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]
                | | [ (4,2), {}, 0 ]

                | paths:
                | | [ (4,3), {'U'}, 1 ]
                | | [ (5,3), {'UR'}, 1 ]
                | | [ (5,2), {'R'}, 1 ]
                | | [ (5,1), {'DR'}, 1 ]
                | | [ (4,1), {'D'}, 1 ]
                | | [ (3,1), {'DL'}, 1 ]
                | | [ (3,2), {'L'}, 1 ]
                | | [ (3,3), {'UL'}, 1 ]

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                fringe = [ (4,3), (5,3), (5,2), (5,1), (4,1), (3,1), (3,2), (3,3) ]   ->   expanding (4,3)   ->   
                found 6 options   ->   duplicate (4,3) path 5 times
                | paths:
                | | [ (4,3), {'U'}, 1 ]
                | | [ (5,3), {'UR'}, 1 ]
                | | [ (5,2), {'R'}, 1 ]
                | | [ (5,1), {'DR'}, 1 ]
                | | [ (4,1), {'D'}, 1 ]
                | | [ (3,1), {'DL'}, 1 ]
                | | [ (3,2), {'L'}, 1 ]
                | | [ (3,3), {'UL'}, 1 ]
                | | [ (4,3), {'U'}, 1 ]
                | | [ (4,3), {'U'}, 1 ]
                | | [ (4,3), {'U'}, 1 ]
                | | [ (4,3), {'U'}, 1 ]
                | | [ (4,3), {'U'}, 1 ]

                | paths:
                | | [ (5,4), {'U', 'UR'}, 2 ]
                | | [ (5,3), {'UR'}, 1 ]
                | | [ (5,2), {'R'}, 1 ]
                | | [ (5,1), {'DR'}, 1 ]
                | | [ (4,1), {'D'}, 1 ]
                | | [ (3,1), {'DL'}, 1 ]
                | | [ (3,2), {'L'}, 1 ]
                | | [ (3,3), {'UL'}, 1 ]
                | | [ (5,3), {'U', 'R'}, 2 ]
                | | [ (5,2), {'U', 'DR'}, 2 ]
                | | [ (4,2), {'U', 'D'}, 2 ]        if state == start & depth != 0: dont accept
                | | [ (3,2), {'U', 'DL'}, 2 ]
                | | [ (3,3), {'U', 'L'}, 2 ]

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                */
                
    
}