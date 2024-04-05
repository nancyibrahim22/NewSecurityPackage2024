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
            
            string[,] plain_matrix = Key_to_matrix(plainText);
            string[,] key0 = Key_to_matrix(key);

            
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
        public string XORKeyMixColumns(string cell1, string cell2)
        {
            string xor_result = "";

            for (int j = 0; j < 8; j++)
            {
                if (cell1[j] == cell2[j])
                {
                    xor_result += '0';
                }
                else
                {
                    xor_result += '1';
                }
            }
            return xor_result;
        }
        string possible_multi_mixColumns(string hexa_row, string hexa_column)
        {
            string result = "";
            string hexa_bin = FromHexToBin(hexa_column);
            if (hexa_row == "01")
            {
                result = hexa_bin;
            }
            else if (hexa_row == "02")
            {
                if (hexa_bin[0] == '0')
                {
                    for (int j = 1; j < 8; j++)
                    {
                        result += hexa_bin[j];
                    }
                    result += '0';
                }
                else
                {
                    string cell_bin_02_after_multi = "";
                    for (int j = 1; j < 8; j++)
                    {
                        cell_bin_02_after_multi += hexa_bin[j];
                    }
                    cell_bin_02_after_multi += '0';
                    string bin_1B = FromHexToBin("1B");
                    result = XORKeyMixColumns(bin_1B, cell_bin_02_after_multi);
                }
            }
            else
            {
                if (hexa_bin[0] == '0')
                {
                    string cell_bin_03_after_multi = "";
                    for (int j = 1; j < 8; j++)
                    {
                        cell_bin_03_after_multi += hexa_bin[j];
                    }
                    cell_bin_03_after_multi += '0';
                    result = XORKeyMixColumns(cell_bin_03_after_multi, hexa_bin);
                }
                else
                {
                    string cell_bin_03_after_multi = "";
                    for (int j = 1; j < 8; j++)
                    {
                        cell_bin_03_after_multi += hexa_bin[j];
                    }
                    cell_bin_03_after_multi += '0';
                    string bin_1B_03 = FromHexToBin("1B");
                    string XOR_1B_03 = XORKeyMixColumns(bin_1B_03, cell_bin_03_after_multi);
                    result = XORKeyMixColumns(XOR_1B_03, hexa_bin);
                }
            }
            return result;
        }
        string[,] mixColumn(string[,] shiftedRows)
        {
            string[] first_first_result = new string[4];
            string[,] mixcolumnn = {{"02", "03", "01", "01"},
                            {"01", "02", "03", "01"},
                            {"01", "01", "02", "03"},
                            {"03", "01", "01", "02"}
                            };
            string[,] result = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        string cell = possible_multi_mixColumns(mixcolumnn[i, k], shiftedRows[k, j]);
                        first_first_result[k] = cell;
                    }
                    string first_half_first_first_XOR = XORKeyMixColumns(first_first_result[0], first_first_result[1]);
                    string sec_half_first_first_XOR = XORKeyMixColumns(first_first_result[2], first_first_result[3]);
                    string pla_result = XORKeyMixColumns(first_half_first_first_XOR, sec_half_first_first_XOR);
                    string hexa_result = FromBinToHex(pla_result);
                    result[i, j] = hexa_result;
                }
            }
            return result;
        }
        public string[,] AddRoundKey(string[,] matrix1, string[,] matrix2)
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
        public string[,] SubBytesRounds(string[,] matrix)
        {
            string[,] subBytes_matrix = new string[4, 4];
            string[,] matrix_first_column = new string[4, 1];
            string[,] matrix_sec_column = new string[4, 1];
            string[,] matrix_third_column = new string[4, 1];
            string[,] matrix_forth_column = new string[4, 1];

            for (int i = 0; i < 4; i++)
            {
                matrix_first_column[i, 0] = matrix[i, 0];
                matrix_sec_column[i, 0] = matrix[i, 1];
                matrix_third_column[i, 0] = matrix[i, 2];
                matrix_forth_column[i, 0] = matrix[i, 3];
            }

            string[,] first_column_result = SubBytesKey(matrix_first_column);
            string[,] sec_column_result = SubBytesKey(matrix_sec_column);
            string[,] third_column_result = SubBytesKey(matrix_third_column);
            string[,] forth_column_result = SubBytesKey(matrix_forth_column);

            for (int i = 0; i < 4; i++)
            {
                subBytes_matrix[i, 0] = first_column_result[i, 0];
                subBytes_matrix[i, 1] = sec_column_result[i, 0];
                subBytes_matrix[i, 2] = third_column_result[i, 0];
                subBytes_matrix[i, 3] = forth_column_result[i, 0];
            }
            return subBytes_matrix;
        }
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
            string[,] sub = SubBytesRounds(prev_result);
            string[,] shiftedRows = shiftRow(sub);
            string[,] roundKey = AddRoundKey(shiftedRows, current_key);
            return roundKey;
        }
        public string[,] InitialRound(string[,] plain_text_matrix, string[,] key_matrix)
        {
            string[,] roundKey0 = AddRoundKey(plain_text_matrix, key_matrix);
            return roundKey0;
        }
        public string[,] RoundsResult(string[,] prev_result, string[,] current_key)
        {
            string[,] sub = SubBytesRounds(prev_result);
            string[,] shiftedRows = shiftRow(sub);
            string[,] mixColumns = mixColumn(shiftedRows);
            string[,] roundKey = AddRoundKey(mixColumns, current_key);
            return roundKey;
        }
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
        public string[,] SubBytesKey(string[,] matrix)
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
                string cell = matrix[i, 0];
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
