/* This file contains all of the test cases for testing the functions
 * in the Program file.
 */
using System.Diagnostics;
using static Trurene_RPG.Program;

namespace Trurene_RPG
{
    class UnitTesting
    {
        public static void RunUnitTesting() {
            // Run tests to ensure the validity of following unit tests
            Debug.Assert(DistanceBetween(world.aurora.pos, world.trollKing.pos) > 0);

            // DistanceBetween
            Position pos1 = new Position();
            Position pos2 = new Position();
            pos1.row = 1;
            pos1.col = 1;
            pos2.row = 5;
            pos2.col = 5;
            Debug.Assert(DistanceBetween(pos1, pos2) == 8);
            Debug.Assert(DistanceBetween(pos2, pos1) == 8);

            // CalcValidMoves
            foreach (Position pos in CalcValidMoves(world.aurora.pos))
            {
                Debug.Assert(DistanceBetween(pos, world.aurora.pos) == 1);
            }

            // MoveCloserTo
            foreach (Position pos in MoveCloserTo(world.aurora.pos, world.trollKing.pos))
            {
                Debug.Assert(DistanceBetween(pos, world.aurora.pos) == 1);
            }

            // CharacterToCreature
            Debug.Assert(CharacterToCreature(world.aurora).attack == world.aurora.attack);
            Debug.Assert(CharacterToCreature(world.aurora).health == world.aurora.health);
            Debug.Assert(CharacterToCreature(world.aurora).maxHealth == world.aurora.maxHealth);
            
        }
    }
}
