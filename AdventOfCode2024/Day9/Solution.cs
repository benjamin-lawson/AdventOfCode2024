using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2024.Day9
{
    public struct DiskChunk
    {
        public int Start;
        public int End;
        public int FileId;

        public int Length => End - Start + 1;
        public bool IsEmpty => FileId == -1;

        public DiskChunk(int start, int end, int fileId)
        {
            Start = start;
            End = end;
            FileId = fileId;
        }

        public override string ToString()
        {
            string charToWrite = IsEmpty ? "." : FileId.ToString();
            string result = "";
            for (int i = 0; i < Length; i++) result += charToWrite;
            return result;
        }
    }

    public class Solution : ISolution
    {
        public void SolvePart1(string[] inputData)
        {
            string textInput = inputData[0].Trim();
            int[] values = ExpandCompactFormat(textInput);

            for (int i = values.Length - 1; i >= 0; i--)
            {
                if (values[i] == -1) continue;

                int firstEmptyBlock = Array.IndexOf(values, -1);
                SwapCharacters(values, i, firstEmptyBlock);

                if (!AnyGapsExist(values, i)) break;
            }

            Console.WriteLine($"Checksum: {GetChecksum(values)}");
        }

        public int[] ExpandCompactFormat(string input)
        {
            List<int> result = new List<int>();
            int fileId = 0;
            bool isFileBlock = true;

            foreach (char c in input)
            {
                int amount = int.Parse(c.ToString());
                int valToWrite = isFileBlock ? fileId : -1;

                for (int i = 0; i < amount; i++)
                {
                    result.Add(valToWrite);
                }

                if (isFileBlock) fileId++;

                isFileBlock = !isFileBlock;
            }

            return result.ToArray();
        }

        public List<DiskChunk> ChunkifyDisk(int[] values)
        {
            List<DiskChunk> result = new List<DiskChunk>();
            int startChunk = 0;
            int currId = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] != currId)
                {
                    result.Add(new DiskChunk(startChunk, i - 1, currId));
                    currId = values[i];
                    startChunk = i;
                }
            }
            
            result.Add(new DiskChunk(startChunk, values.Length - 1, currId));

            return result;
        }

        public void SwapCharacters(int[] text, int i1, int i2)
        {
            int temp = text[i1];
            text[i1] = text[i2];
            text[i2] = temp;
        }

        public void SwapChunk(int[] values, DiskChunk fileChunk, DiskChunk emptyChunk)
        {   
            for (int i = 0; i < fileChunk.Length; i++)
            {
                SwapCharacters(values, fileChunk.Start + i, emptyChunk.Start + i);
            }
        }

        public bool AnyGapsExist(int[] text, int i)
        {
            return text.Take(i).Contains(-1);
        }

        public long GetChecksum(int[] values)
        {
            long checksum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == -1) continue;
                checksum += i * values[i];
            }
            return checksum;
        }

        public IEnumerable<DiskChunk> GetEmptyChunks(List<DiskChunk> chunks, DiskChunk compareChunk)
        {
            return chunks.Where(c => c.End < compareChunk.Start && c.IsEmpty && c.Length >= compareChunk.Length);
        }

        public void SolvePart2(string[] inputData)
        {
            string textInput = inputData[0].Trim();
            int[] values = ExpandCompactFormat(textInput);
            List<DiskChunk> chunks = ChunkifyDisk(values);
            int maxFileId = chunks.Max(c => c.FileId);

            for (int i = maxFileId; i >= 0; i--)
            {
                DiskChunk chunk = chunks.First(c => c.FileId == i);

                if (chunk.Start == 0) break;

                var emptyValidChunks = GetEmptyChunks(chunks, chunk);

                if (emptyValidChunks.Count() == 0) continue;

                SwapChunk(values, chunk, emptyValidChunks.First());

                chunks = ChunkifyDisk(values);
            }

            Console.WriteLine($"Checksum: {GetChecksum(values)}");

        }
    }
}
