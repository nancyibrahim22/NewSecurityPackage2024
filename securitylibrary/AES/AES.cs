using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string[,] plain = StringMatrixConverter(plainText);
            string[,] key1 = StringMatrixConverter(key);
            string[,] binPlain = ConvertToBinary(plain);
            string[,] binkey = ConvertToBinary(key1);
            string[,] roundKey = AddRoundKey(binPlain, binkey);
            string[,] hexa = ConvertToHex(roundKey);
            for (int i = 0; i < 9; i++)
            {
                string[,] sub = subBytes(hexa);
                string[,] shiftedRows = shiftRow(sub);
                string[,] mix = mixColumn(shiftedRows);
                //el mafrod add round key l mix m3 el result bta3 el key expansion
            }









        }
        public [,] string StringMatrixConverter(string toConvert)
        {
            string[,] matrix1 = new string[4, 4];

            int z = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    matrix1[j, i] = toConvert[z] + toConvert[z + 1];



                    z += 2;
                }


            }
            return matrix1;
        }
        public [,] string ConvertToBinary([,] string hexa)
        {
            Dictionary<char, string> hexToBinary = new Dictionary<char, string>()
        {
            {'0', "0000"},
            {'1', "0001"},
            {'2', "0010"},
            {'3', "0011"},
            {'4', "0100"},
            {'5', "0101"},
            {'6', "0110"},
            {'7', "0111"},
            {'8', "1000"},
            {'9', "1001"},
            {'A', "1010"},
            {'B', "1011"},
            {'C', "1100"},
            {'D', "1101"},
            {'E', "1110"},
            {'F', "1111"}
        };
            string[,] binary;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    char letter1 = hexa[i, j][0];
                    char letter2 = hexa[i, j][1];
                    string val1 = hexToBinary[letter1];
                    val1 += hexToBinary[letter2];
                    binary[i, j] = val1;

                }
            }


            return binary;
        }
        public [,] string ConvertToHex(string[,] bin)
        {
            Dictionary<string, string> binaryToHex = new Dictionary<string, string>()
        {
            {"0000", "0"},
            {"0001", "1"},
            {"0010", "2"},
            {"0011", "3"},
            {"0100", "4"},
            {"0101", "5"},
            {"0110", "6"},
            {"0111", "7"},
            {"1000", "8"},
            {"1001", "9"},
            {"1010", "A"},
            {"1011", "B"},
            {"1100", "C"},
            {"1101", "D"},
            {"1110", "E"},
            {"1111", "F"}
};
            string hexa;
            string[,] hex = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < bin[i, j].Length; i += 4)
                    {
                        string val = bin[i, j].Substring(i, 4);
                        hexa += binaryToHex[val];

                    }

                    hex[i, j] = hexa;

                }
            }
            return hex;
        }
        public [,] string binToDecimal(string[,] bin)
        { 
            string[,] decimals= new string[4, 4];
            for (int i = 0; i < 4; i++) 
            {
                for (int j = 0; j < 4:j++)
                {
                    int decimalValue = 0;
                    // initializing base1 value to 1, i.e 2^0 
                    int base1 = 1;
                    int binaryNumbeer = (int)(bin[i, j]);
                    while (binaryNumbeer > 0)
                    {
                        
                        int reminder = binaryNumbeer % 10;
                        binaryNumbeer = binaryNumbeer / 10;
                        decimalValue += reminder * base1;
                        base1 = base1 * 2;
                    }

                    decimals[i, j] = decimalValue;

                }
                    
            }
            return decimals;
        }
        public [,] string decToBinary(string[,] dec)
        {
            int remainder;
            string result = string.Empty;
            while (decimalNumber > 0)
            {
                remainder = decimalNumber % 2;
                decimalNumber /= 2;
                result = remainder.ToString() + result;
            }
        }
        public [,] string AddRoundKey(string[,] matrix1, string[,] matrix2)
        {
            string[,] XOR = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string answer = "";
                    for (int z = 0; z < matrix1[i, j].Length; z++)
                    {
                        if (matrix1[i, j][z] == matrix2[i, j][z])
                        {
                            answer += '0'
                        }
                        else
                        {
                            answer += "1"
                        }
                    }
                    XOR[i, j] = answer;
                }
            }
            return XOR;
        }
        public [,] string subBytes(string[,] subora)
        {
            string[,] sbox = new string[16, 16];
            string[,] result = new string[4, 4];
            sbox =[["63", "7c", "77", "7b", "f2", "6b", "6f", "c5", "30", "01", "67", "2b", "fe", "d7", "ab", "76"],
        ["ca", "82", "c9", "7d", "fa", "59", "47", "f0", "ad", "d4", "a2", "af", "9c", "a4", "72", "c0"],
        ["b7", "fd", "93", "26", "36", "3f", "f7", "cc", "34", "a5", "e5", "f1", "71", "d8", "31", "15"],
        ["04", "c7", "23", "c3", "18", "96", "05", "9a", "07", "12", "80", "e2", "eb", "27", "b2", "75"],
        ["09", "83", "2c", "1a", "1b", "6e", "5a", "a0", "52", "3b", "d6", "b3", "29", "e3", "2f", "84"],
        ["53", "d1", "00", "ed", "20", "fc", "b1", "5b", "6a", "cb", "be", "39", "4a", "4c", "58", "cf"],
        ["d0", "ef", "aa", "fb", "43", "4d", "33", "85", "45", "f9", "02", "7f", "50", "3c", "9f", "a8"],
        ["51", "a3", "40", "8f", "92", "9d", "38", "f5", "bc", "b6", "da", "21", "10", "ff", "f3", "d2"],
        ["cd", "0c", "13", "ec", "5f", "97", "44", "17", "c4", "a7", "7e", "3d", "64", "5d", "19", "73"],
        ["60", "81", "4f", "dc", "22", "2a", "90", "88", "46", "ee", "b8", "14", "de", "5e", "0b", "db"],
        ["e0", "32", "3a", "0a", "49", "06", "24", "5c", "c2", "d3", "ac", "62", "91", "95", "e4", "79"],
        ["e7", "c8", "37", "6d", "8d", "d5", "4e", "a9", "6c", "56", "f4", "ea", "65", "7a", "ae", "08"],
        ["ba", "78", "25", "2e", "1c", "a6", "b4", "c6", "e8", "dd", "74", "1f", "4b", "bd", "8b", "8a"],
        ["70", "3e", "b5", "66", "48", "03", "f6", "0e", "61", "35", "57", "b9", "86", "c1", "1d", "9e"],
        ["e1", "f8", "98", "11", "69", "d9", "8e", "94", "9b", "1e", "87", "e9", "ce", "55", "28", "df"],
        ["8c", "a1", "89", "0d", "bf", "e6", "42", "68", "41", "99", "2d", "0f", "b0", "54", "bb", "16"]];
            Dictionary<char, string> hexTodecimal = new Dictionary<char, int>()
        {  {'A', 10},
            {'B', 11},
            {'C', 12},
            {'D', 13},
            {'E', 14},
            {'F', 15}
        };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int row;
                    int column;
                    if (subora[i, j][0] == 'A' || subora[i, j][0] == 'B' || subora[i, j][0] == 'C' || subora[i, j][0] == 'D' || subora[i, j][0] == 'E' || subora[i, j][0] == 'F')
                    {
                        row = hexTodecimal[subora[i, j][0]];
                    }
                    else if (subora[i, j][0] != 'A' || subora[i, j][0] != 'B' || subora[i, j][0] != 'C' || subora[i, j][0] != 'D' || subora[i, j][0] != 'E' || subora[i, j][0] != 'F')
                    {
                        row = (int)subora[i, j][0];
                    }
                    else if (subora[i, j][1] == 'A' || subora[i, j][1] == 'B' || subora[i, j][1] == 'C' || subora[i, j][1] == 'D' || subora[i, j][1] == 'E' || subora[i, j][1] == 'F')
                    {
                        column = hexTodecimal[subora[i, j][1]];

                    }
                    else if (subora[i, j][1] != 'A' || subora[i, j][1] != 'B' || subora[i, j][1] != 'C' || subora[i, j][1] != 'D' || subora[i, j][1] != 'E' || subora[i, j][1] != 'F')
                    {
                        column = (int)subora[i, j][1];
                    }
                    result[i, j] = sbox[row, column];
                }
            }
            return result;
        }
        public [,] string shiftRow(string[,] subed)
        {

            string[,] result = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                { if (i == 0)
                    {
                        result[i, j] = subed[i, j]
                    }
                    else
                    {
                        result[i, j] = subed[i, (j + i) % 4];
                    }

                }


            }

            return result;


        }
        public [,] string mixColumn(string[,] shiftedRows)
        {
            string[,] mixcolumnn = [["02", "03", "01", "01"],
                                    ["01", "02", "03", "01"],
                                    ["01", "01", "02", "03"],
                                    ["03", "01", "01", "02"]
                                    ];
            string[,] result = new string[4, 4];
            string[,] binshift = new string[4, 4];
            string[,] mixcolum = new string[4, 4];
            binshift = ConvertToBinary(shiftedRows);
            mixcolum = ConvertToBinary(mixcolumnn);
            //string[,] shiftdec=binToDecimal(binshift);
            //string[,] mixdec = binToDecimal(mixcolum);


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int[] arr = new int[4];
                    for (int k = 0; k < 4; k++)
                    {   
                        int dec1 = Convert.ToInt32(mixcolum[i, k], 2);
                        int dec2 = Convert.ToInt32(binshift[k, j], 2);
                        if (dec1 * dec2 > 255)
                        {
                            int tobXor = dec1 * dec2;
                            int hex1b = 0x1B;
                            int result = tobXor ^ hex1b;
                            //string binaryResult = Convert.ToString(result, 2);
                            arr[k]=result;

                        }
                        else
                        {
                            int result = dec2 * dec1;
                            arr[k]=result;
                            //string binaryResult = Convert.ToString(result, 2);
                        }
                    }
                    int res = arr[0]^arr[1]^arr[2]^arr[3];
                    result[i, j] = Convert.ToString(res, 2);
                
                }           
            }
        }
    }
}
