﻿namespace dp.DpxInstructionSet;

// 
// All opcodes are 8-bit values
// https://uefi.org/specs/PI/1.8/V1_PEI_Foundation.html#dependency-expression-instruction-set
//
public enum Opcodes : sbyte
{
    //
    // Pushes a Boolean value onto the stack. 
    // If the GUID is present in the handle database, then a TRUE is pushed onto the stack. 
    // If the GUID is not present in the handle database, then a FALSE is pushed onto the stack. 
    // The test for the GUID in the handle database may be performed with the Boot Service LocatePpi() .       
    //
    PUSH = 0x2,

    //
    // Pops two Boolean operands off the stack, performs a Boolean AND operation between the two operands, and pushes the result back onto the stack.
    //
    AND = 0x3,

    //
    // Pops two Boolean operands off the stack, performs a Boolean OR operation between the two operands, and pushes the result back onto the stack.
    //
    OR = 0x4,

    //
    // Pops a Boolean operands off the stack, performs a Boolean NOT operation on the operand, and pushes the result back onto the stack.
    //
    NOT = 0x5,

    //
    // Pushes a Boolean TRUE onto the stack.
    //
    TRUE = 0x6,

    //
    // Pushes a Boolean FALSE onto the stack.
    // 
    FALSE = 0x7,

    //
    // Pops the final result of the dependency expression evaluation off the stack and exits the dependency expression evaluator.
    // 
    END = 0x8
}

internal class DpxInstructionSet
{
    public static string MnemonicFromOpcode(Opcodes opcode)
    {
        return opcode switch
        {
            Opcodes.PUSH => "PUSH",
            Opcodes.AND => "AND",
            Opcodes.OR => "OR",
            Opcodes.NOT => "NOT",
            Opcodes.TRUE => "TRUE",
            Opcodes.FALSE => "FALSE",
            Opcodes.END => "END",
            _ => throw new ArgumentOutOfRangeException(nameof(opcode), @$"[-] Invalid Opcode {opcode}")
        };
    }

}
