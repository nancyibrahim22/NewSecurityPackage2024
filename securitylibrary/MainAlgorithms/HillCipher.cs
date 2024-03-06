using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {

            throw new NotImplementedException();
        }
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
        
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> result = new List<int>();
            int[,] InverseKeyMatrix = new int[3, 3];
            int[,] InverseKeyMatrixTrans = new int[3, 3];
            //int[,] Cipher2d = new int[cipherText.Count/3, 3];

            int step_size,det;

            if (key.Count == 4)
                step_size = 2;
            else
                step_size = 3;

            #region DetAndInverse
            if (key.Count == 4)
                det = key[0] * key[3] - key[1] * key[2];
            else
            {
                det = key[0] * (key[4] * key[8] - key[5] * key[7])
                    - key[1] * (key[3] * key[8] - key[5] * key[6])
                    + key[2] * (key[3] * key[7] - key[4] * key[6]);

            }
            det = ((det % 26) + 26) % 26;
            Console.WriteLine(det);
            int inverse = InverseFinder(det);
            Console.WriteLine(inverse);
            #endregion

            if (step_size==3)
            {

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int pow = i + j;
                        if (pow % 2 == 0)
                            pow = 1;
                        else
                            pow = -1;

                        int detHere = getDet(key, i, j);
                        // InverseKeyMatrix[i,j]=((inverse*pow*detHere)%26);
                        int Eq = inverse * pow * detHere;
                        InverseKeyMatrix[i, j] = ((Eq % 26) + 26) % 26;

                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        InverseKeyMatrixTrans[i, j] = InverseKeyMatrix[j, i];
                        Console.Write($"{InverseKeyMatrixTrans[i, j]} ");
                    }
                    Console.WriteLine();

                }

                for (int i = 0; i < cipherText.Count; i += 3)
                {
                    result.Add((cipherText[i] * InverseKeyMatrixTrans[0, 0]
                        + cipherText[i + 1] * InverseKeyMatrixTrans[0, 1]
                        + cipherText[i + 2] * InverseKeyMatrixTrans[0, 2]) % 26);

                    result.Add((cipherText[i] * InverseKeyMatrixTrans[1, 0]
                        + cipherText[i + 1] * InverseKeyMatrixTrans[1, 1]
                        + cipherText[i + 2] * InverseKeyMatrixTrans[1, 2]) % 26);

                    result.Add((cipherText[i] * InverseKeyMatrixTrans[2, 0]
                        + cipherText[i + 1] * InverseKeyMatrixTrans[2, 1]
                        + cipherText[i + 2] * InverseKeyMatrixTrans[2, 2]) % 26);
                } 
            }
            else
            {
                int idx = 3;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int pow = i + j;
                        if (pow % 2 == 0)
                            pow = 1;
                        else
                            pow = -1;

                        //int detHere = getDet(key, i, j);
                        // InverseKeyMatrix[i,j]=((inverse*pow*detHere)%26);
                        int Eq = inverse *key[idx] *pow;
                        //Console.WriteLine($"{Eq} ");
                        InverseKeyMatrix[i, j] = ((Eq % 26) + 26) % 26;
                        idx--;
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j <2 ; j++)
                    {
                        InverseKeyMatrixTrans[i, j] = InverseKeyMatrix[j, i];
                        Console.Write($"{InverseKeyMatrixTrans[i, j]} ");
                    }
                    Console.WriteLine();

                }

                for (int i = 0; i < cipherText.Count; i += 2)
                {
                    result.Add((cipherText[i] * InverseKeyMatrixTrans[0, 0]
                        + cipherText[i + 1] * InverseKeyMatrixTrans[0, 1]
                        ) % 26);

                    result.Add((cipherText[i] * InverseKeyMatrixTrans[1, 0]
                        + cipherText[i + 1] * InverseKeyMatrixTrans[1, 1]
                        ) % 26);

                   
                }
            }
            foreach (var x in result)
                Console.Write($"{x} ");
            return result;           
            // throw new NotImplementedException();
        }
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            //throw new NotImplementedException();
            List<int> result = new List<int>();

            int i = 0;
            int step_size = 0;
            int j = 0;
            if (key.Count == 4)
                step_size = 2;
            else
                step_size = 3;
            while (i < plainText.Count)
            {
                //Console.WriteLine(i);
                if (step_size==2)
                {
                    if (i % 2 == 0)
                    {
                        j = 0;
                        int x = plainText[i] * key[j] + plainText[i + 1] * key[j + 1];
                        x = x % 26;
                        result.Add(x);
                        //Console.WriteLine(x);
                    }
                    else
                    {
                        j = 2;
                        int y = plainText[i - 1] * key[j] + plainText[i] * key[j + 1];
                        y = y % 26;
                        result.Add(y);
                        //Console.WriteLine(y);
                    } 
                }

                else
                {
                    if (i % 3 == 0)
                    {
                        j = 0;
                        int x = plainText[i] * key[j] + plainText[i + 1] * key[j + 1]
                            +plainText[i+2]*key[j+2];
                        x = x % 26;
                        result.Add(x);
                        //Console.WriteLine(x);
                    }
                    else if(i%3==1)
                    {
                        j = 3;
                        int x = plainText[i-1] * key[j] + plainText[i] * key[j + 1]
                           + plainText[i + 1] * key[j + 2];
                        x = x % 26;
                        result.Add(x);
                    }
                    else
                    {
                        j = 6;
                        int x = plainText[i-2] * key[j] + plainText[i - 1] * key[j + 1]
                           + plainText[i ] * key[j + 2];
                        x = x % 26;
                        result.Add(x);
                       // result.Add(y);
                        //Console.WriteLine(y);
                    }
                }

                //j++;
                //j%=step_size;
                i++;
            }
            //foreach(var x in result)
            //    Console.WriteLine(x);
            return result;
        }
        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {

            throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }

        public int InverseFinder(int determinant)
        {
            //List<int> result = new List<int>();
            //int result;
            int a1 = 1,a2=0,a3=26;
            int b1=0,b2=1,b3=determinant;
            int q ;
            int t1 ;
            int t2 ;
            int t3 ;
            while (true)
            {
                if(b3==0)
                {
                    throw new System.Exception() ;
                }
                else if(b3==1)
                {
                    return ((b2 % 26) + 26) % 26;
                }
                q = a3 / b3;
                t1 = a1 - (q * b1);
                t2 = a2 - (q * b2);
                t3 = a3 - (q * b3);
                a1 =b1; a2=b2; a3=b3;
                b1=t1; b2=t2; b3=t3;
                Console.WriteLine($"{t1} {t2} {t3}");

            }
            //return 0;
        }

        public int getDet(List<int> values,int x,int y)
        {
            int ans ;
            List<int> sub = new List<int>();
            int  myiteratoridx=0;
            int[,] arr = new int[3, 3];
            // Now we have 2d Array
            for(int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    arr[i, j] = values[myiteratoridx];
                    myiteratoridx++;
                }
            }

            // Construct Sub array
            for(int i=0;i<3;i++)
            {
                for(int j=0; j<3;j++)
                {
                    if(i!=x&&j!=y)
                        sub.Add(arr[i,j]);
                }
            }

            ans = (sub[0] * sub[3]) - (sub[1] * sub[2]);
            //ans %= 26;

            return ans;
        }

        
    }
}

