using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOLRule  {
    private int inst1;
    private int inst2;
    private int inst3;
    private int inst4;
    private int[] instructions = new int[4];

	// Use this for initialization
	public void setupRule(int _inst0, int _inst1, int _inst2, int _inst3)
    {
        instructions[0] = _inst0;
        instructions[1] = _inst1;
        instructions[2] = _inst2;
        instructions[3] = _inst3;
    }

    public int getInstruction(int _index)
    {
        return instructions[_index];
    }

    public void setInstruction(int _inst, int _index)
    {
        instructions[_index] = _inst;
    }

    public int[] getInstructions()
    {
        return instructions;
    }
}
