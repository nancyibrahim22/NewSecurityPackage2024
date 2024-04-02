using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            //string[,] plain = StringMatrixConverter(plainText);
            //string[,] key1 = StringMatrixConverter(key);
            //string[,] binPlain = ConvertToBinary(plain);
            //string[,] binkey = ConvertToBinary(key1);
            //string[,] roundKey = AddRoundKey(binPlain, binkey);
            //string[,] hexa = ConvertToHex(roundKey);
            //for (int i = 0; i < 9; i++)
            //{
            //    string[,] sub = subBytes(hexa);
            //    string[,] shiftedRows = shiftRow(sub);
            //    string[,] mix = mixColumn(shiftedRows);
            //    //el mafrod add round key l mix m3 el result bta3 el key expansion
            //}
            

            //NONOOOOOOOS
            //convert plain and key to matrices
            string[,] plain_matrix = StringMatrixConverter(plainText);
            string[,] key0 = Key_to_matrix(key);

            //key expansion all rounds
            string[,] key1 = KeyExpansion(key0, 0);
            string[,] key2 = KeyExpansion(key1, 1);
            string[,] key3 = KeyExpansion(key2, 2);
            string[,] key4 = KeyExpansion(key3, 3);
            string[,] key5 = KeyExpansion(key4, 4);
            string[,] key6 = KeyExpansion(key5, 5);
            string[,] key7 = KeyExpansion(key6, 6);
            string[,] key8 = KeyExpansion(key7, 7);
            string[,] key9 = KeyExpansion(key8, 8);
            string[,] key10 = KeyExpansion(key9, 9);

            //initial round 
            string[,] roundKey0 = InitialRound(plain_matrix, key0);
            //round 1
            string[,] roundKey1 = RoundsResult(roundKey0, key1);
            //round 2
            string[,] roundKey2 = RoundsResult(roundKey1, key2);
            //round 3
            string[,] roundKey3 = RoundsResult(roundKey2, key3);
            //round 4
            string[,] roundKey4 = RoundsResult(roundKey3, key4);
            //round 5
            string[,] roundKey5 = RoundsResult(roundKey4, key5);
            //round 6
            string[,] roundKey6 = RoundsResult(roundKey5, key6);
            //round 7
            string[,] roundKey7 = RoundsResult(roundKey6, key7);
            //round 8
            string[,] roundKey8 = RoundsResult(roundKey7, key8);
            //round 9
            string[,] roundKey9 = RoundsResult(roundKey8, key9);
            //round 10
            string[,] roundKey10 = LastRoundResult(roundKey9, key10);

            string cipherText = MatrixToString(roundKey10);
            return cipherText;
        }
        public string [,] StringMatrixConverter(string toConvert)
        {
            string[,] matrix1 = new string[4, 4];

            int z = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    matrix1[j, i] = toConvert[z] +"" + toConvert[z + 1];

                    z += 2;
                }


            }
            return matrix1;
        }
        public string[,] ConvertToBinary(string[,] hexa)
        {
            string[,] binary = new string[4, 4];
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
        public string[,] ConvertToHex(string[,] bin)
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
        public string[,] binToDecimal(string[,] bin)
        { 
            string[,] decimals= new string[4, 4];
            for (int i = 0; i < 4; i++) 
            {
                for (int j = 0; j < 4; j++)
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
        public string[,] decToBinary(string[,] dec)
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
        public string[,] AddRoundKey(string[,] matrix1, string[,] matrix2)
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
                            answer += '0';
                        }
                        else
                        {
                            answer += "1";
                        }
                    }
                    XOR[i, j] = answer;
                }
            }
            return XOR;
        }
        public string[,] subBytes(string[,] subora)
        {
            string[,] result = new string[4, 4];
            string[,] sbox = new string[16, 16]
            {
                {"63", "7c", "77", "7b", "f2", "6b", "6f", "c5", "30", "01", "67", "2b", "fe", "d7", "ab", "76"},
                {"ca", "82", "c9", "7d", "fa", "59", "47", "f0", "ad", "d4", "a2", "af", "9c", "a4", "72", "c0"},
                {"b7", "fd", "93", "26", "36", "3f", "f7", "cc", "34", "a5", "e5", "f1", "71", "d8", "31", "15"},
                {"04", "c7", "23", "c3", "18", "96", "05", "9a", "07", "12", "80", "e2", "eb", "27", "b2", "75"},
                {"09", "83", "2c", "1a", "1b", "6e", "5a", "a0", "52", "3b", "d6", "b3", "29", "e3", "2f", "84"},
                {"53", "d1", "00", "ed", "20", "fc", "b1", "5b", "6a", "cb", "be", "39", "4a", "4c", "58", "cf"},
                {"d0", "ef", "aa", "fb", "43", "4d", "33", "85", "45", "f9", "02", "7f", "50", "3c", "9f", "a8"},
                {"51", "a3", "40", "8f", "92", "9d", "38", "f5", "bc", "b6", "da", "21", "10", "ff", "f3", "d2"},
                {"cd", "0c", "13", "ec", "5f", "97", "44", "17", "c4", "a7", "7e", "3d", "64", "5d", "19", "73"},
                {"60", "81", "4f", "dc", "22", "2a", "90", "88", "46", "ee", "b8", "14", "de", "5e", "0b", "db"},
                {"e0", "32", "3a", "0a", "49", "06", "24", "5c", "c2", "d3", "ac", "62", "91", "95", "e4", "79"},
                {"e7", "c8", "37", "6d", "8d", "d5", "4e", "a9", "6c", "56", "f4", "ea", "65", "7a", "ae", "08"},
                {"ba", "78", "25", "2e", "1c", "a6", "b4", "c6", "e8", "dd", "74", "1f", "4b", "bd", "8b", "8a"},
                {"70", "3e", "b5", "66", "48", "03", "f6", "0e", "61", "35", "57", "b9", "86", "c1", "1d", "9e"},
                {"e1", "f8", "98", "11", "69", "d9", "8e", "94", "9b", "1e", "87", "e9", "ce", "55", "28", "df"},
                {"8c", "a1", "89", "0d", "bf", "e6", "42", "68", "41", "99", "2d", "0f", "b0", "54", "bb", "16"}
            };
            Dictionary<char, int> hexTodecimal = new Dictionary<char, int>()
            {  
                {'A', 10},
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
                    int row=0;
                    int column=0;
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
        public string[,] shiftRow(string[,] subed)
        {

            string[,] result = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0)
                    {
                        result[i, j] = subed[i, j];
                    }
                    else
                    {
                        result[i, j] = subed[i, (j + i) % 4];
                    }

                }
            }

            return result;


        }
        public string[,] mixColumn(string[,] shiftedRows)
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


        //nonos add round key
        public string[,] AddRoundKey2(string[,] matrix1, string[,] matrix2)
        {
            string[,] round_key = new string[4, 4];
            string[,] matrix1_first_column = new string[4, 1];
            string[,] matrix1_sec_column = new string[4, 1];
            string[,] matrix1_third_column = new string[4, 1];
            string[,] matrix1_forth_column = new string[4, 1];
            string[,] matrix2_first_column = new string[4, 1];
            string[,] matrix2_sec_column = new string[4, 1];
            string[,] matrix2_third_column = new string[4, 1];
            string[,] matrix2_forth_column = new string[4, 1];
            for (int i = 0; i < 4; i++)
            {
                matrix1_first_column[i,0] = matrix1[i, 0];
                matrix1_sec_column[i,0] = matrix1[i, 1];
                matrix1_third_column[i,0] = matrix1[i, 2];
                matrix1_forth_column[i,0] = matrix1[i, 3];

                matrix2_first_column[i, 0] = matrix2[i, 0];
                matrix2_sec_column[i, 0] = matrix2[i, 1];
                matrix2_third_column[i, 0] = matrix2[i, 2];
                matrix2_forth_column[i, 0] = matrix2[i, 3];
            }
            string[,] first_column_result = XORKey(matrix1_first_column, matrix2_first_column);
            string[,] sec_column_result = XORKey(matrix1_sec_column, matrix2_sec_column);
            string[,] third_column_result = XORKey(matrix1_third_column, matrix2_third_column);
            string[,] forth_column_result = XORKey(matrix1_forth_column, matrix2_forth_column);

            for(int i = 0; i < 4; i++)
            {
                round_key[i,0] = first_column_result[i,0];
                round_key[i,1] = sec_column_result[i,0];
                round_key[i,2] = third_column_result[i,0];
                round_key[i,3] = forth_column_result[i,0];
            }
            return round_key;
        }


        //Nonos needed encrypt functions
        public string MatrixToString(string[,] matrix)
        {
            string text = "0x";
            for(int i=0; i< 4; i++)
            {
                for(int j=0; j< 4; j++)
                {
                    text += matrix[j, i];
                }
            }
            return text;
        }

        public string[,] LastRoundResult(string[,] prev_result, string[,] current_key)
        {
            string[,] sub = subBytes(prev_result);
            string[,] shiftedRows = shiftRow(sub);
            string[,] roundKey = AddRoundKey2(shiftedRows, current_key);
            return roundKey;
        }

        public string[,] InitialRound(string[,] plain_text_matrix, string[,] key_matrix)
        {
            string[,] roundKey0 = AddRoundKey2(plain_text_matrix, key_matrix);
            return roundKey0;
        }

        public string[,] RoundsResult(string[,] prev_result, string[,] current_key)
        {
            string[,] sub = subBytes(prev_result);
            string[,] shiftedRows = shiftRow(sub);
            string[,] mixColumns = mixColumn(shiftedRows);
            string[,] roundKey = AddRoundKey2(mixColumns, current_key);
            return roundKey;
        }


        //nonos key expansion
        public string[,] RotateLastColumn(string[,] key_matrix)
        {
            string[,] rotated_last_column = new string[4, 1];
            rotated_last_column[3, 0] = key_matrix[0, 3];
            for (int i = 0; i < 3; i++)
            {
                rotated_last_column[i, 0] = key_matrix[i + 1, 3];
            }
            return rotated_last_column;
        }

        public string[,] XORKey(string[,] column1, string[,] column2)
        {
            string[,] xor_column = new string[4, 1];

            for (int i = 0; i < 4; i++)
            {
                string column1_cell = column1[i, 0];
                string column2_cell = column2[i, 0];
                string column1_bin = FromHexToBin(column1_cell);
                string column2_bin = FromHexToBin(column2_cell);
                string xor_result = "";
                for (int j = 0; j < 8; j++)
                {
                    if (column1_bin[j] == column2_bin[j])
                    {
                        xor_result += '0';
                    }
                    else
                    {
                        xor_result += '1';
                    }
                }
                string xor_result_hexa = FromBinToHex(xor_result);
                xor_column[i, 0] = xor_result_hexa;
            }
            return xor_column;
        }

        public string FromBinToHex(string binary)
        {
            Dictionary<char, string> hexTobin = new Dictionary<char, string>()
            {
                {'a', "1010"},
                {'b', "1011"},
                {'c', "1100"},
                {'d', "1101"},
                {'e', "1110"},
                {'f', "1111"},
                {'A', "1010"},
                {'B', "1011"},
                {'C', "1100"},
                {'D', "1101"},
                {'E', "1110"},
                {'F', "1111"},
                {'0', "0000"},
                {'1', "0001"},
                {'2', "0010"},
                {'3', "0011"},
                {'4', "0100"},
                {'5', "0101"},
                {'6', "0110"},
                {'7', "0111"},
                {'8', "1000"},
                {'9', "1001"}
            };
            string first_binary = binary[0] + "" + binary[1] + "" + binary[2] + "" + binary[3];
            string sec_binary = binary[4] + "" + binary[5] + "" + binary[6] + "" + binary[7];
            char first_hexa = hexTobin.FirstOrDefault(x => x.Value == first_binary).Key;
            char sec_hexa = hexTobin.FirstOrDefault(x => x.Value == sec_binary).Key;
            string hexa = first_hexa + "" + sec_hexa;
            return hexa;
        }
        public string FromHexToBin(string hexa)
        {
            Dictionary<char, string> hexTobin = new Dictionary<char, string>()
            {
                {'a', "1010"},
                {'b', "1011"},
                {'c', "1100"},
                {'d', "1101"},
                {'e', "1110"},
                {'f', "1111"},
                {'A', "1010"},
                {'B', "1011"},
                {'C', "1100"},
                {'D', "1101"},
                {'E', "1110"},
                {'F', "1111"},
                {'0', "0000"},
                {'1', "0001"},
                {'2', "0010"},
                {'3', "0011"},
                {'4', "0100"},
                {'5', "0101"},
                {'6', "0110"},
                {'7', "0111"},
                {'8', "1000"},
                {'9', "1001"}
            };
            char first_hexa = hexa[0];
            char sec_hexa = hexa[1];
            string first_hexa_bin = hexTobin[first_hexa];
            string sec_hexa_bin = hexTobin[sec_hexa];
            string binary = first_hexa_bin + "" + sec_hexa_bin;

            return binary;
        }

        public string[,] SubBytesKey(string[,] subora)
        {
            string[,] result = new string[4, 1];
            string[,] sbox = new string[16, 16]
            {
                {"63", "7c", "77", "7b", "f2", "6b", "6f", "c5", "30", "01", "67", "2b", "fe", "d7", "ab", "76"},
                {"ca", "82", "c9", "7d", "fa", "59", "47", "f0", "ad", "d4", "a2", "af", "9c", "a4", "72", "c0"},
                {"b7", "fd", "93", "26", "36", "3f", "f7", "cc", "34", "a5", "e5", "f1", "71", "d8", "31", "15"},
                {"04", "c7", "23", "c3", "18", "96", "05", "9a", "07", "12", "80", "e2", "eb", "27", "b2", "75"},
                {"09", "83", "2c", "1a", "1b", "6e", "5a", "a0", "52", "3b", "d6", "b3", "29", "e3", "2f", "84"},
                {"53", "d1", "00", "ed", "20", "fc", "b1", "5b", "6a", "cb", "be", "39", "4a", "4c", "58", "cf"},
                {"d0", "ef", "aa", "fb", "43", "4d", "33", "85", "45", "f9", "02", "7f", "50", "3c", "9f", "a8"},
                {"51", "a3", "40", "8f", "92", "9d", "38", "f5", "bc", "b6", "da", "21", "10", "ff", "f3", "d2"},
                {"cd", "0c", "13", "ec", "5f", "97", "44", "17", "c4", "a7", "7e", "3d", "64", "5d", "19", "73"},
                {"60", "81", "4f", "dc", "22", "2a", "90", "88", "46", "ee", "b8", "14", "de", "5e", "0b", "db"},
                {"e0", "32", "3a", "0a", "49", "06", "24", "5c", "c2", "d3", "ac", "62", "91", "95", "e4", "79"},
                {"e7", "c8", "37", "6d", "8d", "d5", "4e", "a9", "6c", "56", "f4", "ea", "65", "7a", "ae", "08"},
                {"ba", "78", "25", "2e", "1c", "a6", "b4", "c6", "e8", "dd", "74", "1f", "4b", "bd", "8b", "8a"},
                {"70", "3e", "b5", "66", "48", "03", "f6", "0e", "61", "35", "57", "b9", "86", "c1", "1d", "9e"},
                {"e1", "f8", "98", "11", "69", "d9", "8e", "94", "9b", "1e", "87", "e9", "ce", "55", "28", "df"},
                {"8c", "a1", "89", "0d", "bf", "e6", "42", "68", "41", "99", "2d", "0f", "b0", "54", "bb", "16"}
            };
            Dictionary<char, int> hexTodecimal = new Dictionary<char, int>()
            {
                {'A', 10},
                {'B', 11},
                {'C', 12},
                {'D', 13},
                {'E', 14},
                {'F', 15},
                {'a', 10},
                {'b', 11},
                {'c', 12},
                {'d', 13},
                {'e', 14},
                {'f', 15}
            };
            for (int i = 0; i < 4; i++)
            {
                string cell = subora[i, 0];
                char first_char = cell[0];
                char second_char = cell[1];
                if (char.IsLetter(first_char))
                {
                    int row = hexTodecimal[first_char];
                    if (char.IsLetter(second_char))
                    {
                        int column = hexTodecimal[second_char];
                        result[i, 0] = sbox[row, column];
                    }
                    else if (char.IsDigit(second_char))
                    {
                        int column = second_char - '0';
                        result[i, 0] = sbox[row, column];
                    }
                }
                else if (char.IsDigit(first_char))
                {
                    int row = first_char - '0';
                    if (char.IsLetter(second_char))
                    {
                        int column = hexTodecimal[second_char];
                        result[i, 0] = sbox[row, column];
                    }
                    else if (char.IsDigit(second_char))
                    {
                        int column = second_char - '0';
                        result[i, 0] = sbox[row, column];
                    }
                }
            }
            return result;
        }

        public string[,] Key_to_matrix(string key)
        {
            int row = 0;
            int column = 0;
            string[,] key_matrix = new string[4, 4];
            for (int i = 2; i < key.Length; i += 2)
            {
                if (i == key.Length - 1)
                    break;
                string cell = key[i] + "" + key[i + 1];
                key_matrix[row, column] = cell;
                row = (row + 1) % 4;
                if (row == 0)
                {
                    column++;

                }
            }
            return key_matrix;
        }

        public string[,] KeyExpansion(string[,] prev_key_matrix, int round_number)
        {
            string[,] finalKeyMatrix = new string[4, 4];
            string[,] prev_second_column = new string[4, 1];
            string[,] prev_third_column = new string[4, 1];
            string[,] prev_forth_column = new string[4, 1];
            string[,] keyExpansionMatrix = new string[4, 10]
            {
                {"01", "02", "04", "08", "10", "20", "40", "80", "1b", "36"},
                {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
                {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
                {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00"}
            };
            //rotate last column in prev_key_matrix
            string[,] rotated_prev_last_column = RotateLastColumn(prev_key_matrix);

            //subbytes the rotated column
            string[,] subBytes_prev_last_column = SubBytesKey(rotated_prev_last_column);

            //XOR subbyte rotated column with prev first column
            string[,] prev_first_column = new string[4, 1];
            for (int i = 0; i < 4; i++)
            {
                prev_first_column[i, 0] = prev_key_matrix[i, 0];
            }
            string[,] first_xor_rotated = XORKey(subBytes_prev_last_column, prev_first_column);

            //XOR prev_xor_result with 
            string[,] ExpansionMatrixColumn = new string[4, 1];
            for (int i = 0; i < 4; i++)
            {
                ExpansionMatrixColumn[i, 0] = keyExpansionMatrix[i, round_number];
            }
            string[,] new_first_column = XORKey(first_xor_rotated, ExpansionMatrixColumn);

            //second column: xor new_first_column with prev_second_column
            for (int i = 0; i < 4; i++)
            {
                prev_second_column[i, 0] = prev_key_matrix[i, 1];
            }
            string[,] new_second_column = XORKey(new_first_column, prev_second_column);

            //third column: xor new_second_column with prev_third_column
            for (int i = 0; i < 4; i++)
            {
                prev_third_column[i, 0] = prev_key_matrix[i, 2];
            }
            string[,] new_third_column = XORKey(new_second_column, prev_third_column);

            //forth column: xor new_third_column with prev_forth_column
            for (int i = 0; i < 4; i++)
            {
                prev_forth_column[i, 0] = prev_key_matrix[i, 3];
            }
            string[,] new_forth_column = XORKey(new_third_column, prev_forth_column);

            //final matrix
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 0)
                    {
                        finalKeyMatrix[i, j] = new_first_column[i, 0];
                    }
                    else if (j == 1)
                    {
                        finalKeyMatrix[i, j] = new_second_column[i, 0];
                    }
                    else if (j == 2)
                    {
                        finalKeyMatrix[i, j] = new_third_column[i, 0];
                    }
                    else
                    {
                        finalKeyMatrix[i, j] = new_forth_column[i, 0];
                    }
                }
            }
            return finalKeyMatrix;
        }
    }
}
