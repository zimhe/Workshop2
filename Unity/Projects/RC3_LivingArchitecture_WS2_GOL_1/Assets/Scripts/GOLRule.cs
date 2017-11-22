using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOLRule  {
    public int inst1;
    public int inst2;
    public int inst3;
    public int inst4;
    public int[] instructions = new int[4];
	// Use this for initialization
	void setupRule(int _inst0, int _inst1, int _inst2, int _inst3)
    {
        instructions[0] = _inst0;
        instructions[1] = _inst1;
        instructions[2] = _inst2;
        instructions[3] = _inst3;
    }

    int getInstruction(int _index)
    {
        return instructions[_index];
    }

    void setInstruction(int _inst, int _index)
    {
        instructions[_index] = _inst;
    }
}
