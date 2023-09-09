using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MakeInference
{
   int N_FACTORS = 13;
   public int[] getFactorsFromState(int state)
   {
        
        int[] factors = {0,0};
        factors[0] = state / N_FACTORS;
        factors[2] = state % N_FACTORS;

        return factors;
   }

   public int getStateFromFactors(int n1, int n2)
   {
        return (n1 * N_FACTORS)+ n2; 
   }

   //convert factors to state

   //api
   //pass state into qtable to get action


   //convert state to factors

}
