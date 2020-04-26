using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BJ
{
    enum X { w, r, p, n };
    class Player
    {
        public int money;
        public int bet;
        public int counter;
        public int split_counter;

        public List<PictureBox> pic = new List<PictureBox>();
        public List<PictureBox> split_pic = new List<PictureBox>();
        public List<Card> cards = new List<Card>();
        public List<Card> split_cards = new List<Card>();
        List<string> EvaluationTable = new List<string>();
        List<string> SplitTable = new List<string>();

        //liczenie punktow
        public int get_points(List<Card> x)
        {
            List<int> bestresult = new List<int>();
            int result = 0;
            int aces = 0;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].reverse == true)
                    continue;
                result += x[i].value;
                if (x[i].value == 1)
                    aces++;
            }
            //asy
            bestresult.Add(result);
            for (int i = 0; i < aces; i++)
            {
                bestresult.Add(result + (i + 1) * 10);
            }

            bestresult.Sort();
            int tmp = result;
            for (int i = 0; i < bestresult.Count; i++)
            {
                if (bestresult[i] > 21)
                {
                    return tmp;
                }
                tmp = bestresult[i];
            }
            return tmp;
        }

        //dobranie karty
        public void draw(Card x, int count, bool split = false)
        {
            if (split == false)
            {
                cards.Add(x);
                pic[count].Image = x.image;
            }
            else
            {
                split_cards.Add(x);
                split_pic[count].Image = x.image;
            }
        }

        public X Results(Player a, Player b, bool split = false)
        {
            int apoints;
            if (split == false)
                apoints = a.get_points(a.cards);
            else
            {
                apoints = a.get_points(a.split_cards);
            }
            int bpoints = b.get_points(b.cards);


            if (apoints == 0)
                return X.n;

            if (apoints < 22 && a.cards.Count == 7)
            {
                a.money += 2 * a.bet;
                return X.w;
            }


            if (apoints < 22 && bpoints > 21 || (apoints > bpoints && apoints < 22))
            {
                a.money += 2 * a.bet;
                return X.w;
            }
            else if (apoints == bpoints && apoints < 22)
            {
                a.money += a.bet;
                return X.r;
            }
            else
            {
                return X.p;
            }
        }

        //ewaluacja komputera
        public void ev()
        {
            EvaluationTable.Add("08,h,h,h,h,h,h,h,h,h,h");//0
            EvaluationTable.Add("09,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("10,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("11,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("12,h,h,s,s,s,h,h,h,h,h");
            EvaluationTable.Add("13,s,s,s,s,s,h,h,h,h,h");
            EvaluationTable.Add("14,s,s,s,s,s,h,h,h,h,h");
            EvaluationTable.Add("15,s,s,s,s,s,h,h,h,h,h");
            EvaluationTable.Add("16,s,s,s,s,s,h,h,h,h,h");
            EvaluationTable.Add("17,s,s,s,s,s,s,s,s,s,s");
            EvaluationTable.Add("A2,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("A3,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("A4,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("A5,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("A6,h,h,h,h,h,h,h,h,h,h");
            EvaluationTable.Add("A7,s,h,h,h,h,s,s,h,h,h");
            EvaluationTable.Add("A8,s,s,s,s,s,s,s,s,s,s");
            EvaluationTable.Add("A9,s,s,s,s,s,s,s,s,s,s");
            EvaluationTable.Add("A0,s,s,s,s,s,s,s,s,s,s");//18

            //h-dobierz, s-pas, p-split

            SplitTable.Add("22,p,p,p,p,p,p,h,h,h,h");
            SplitTable.Add("33,p,p,p,p,p,p,p,p,p,h");
            SplitTable.Add("44,h,h,h,p,p,h,h,h,h,h");
            SplitTable.Add("55,h,h,h,h,h,h,h,h,h,h");
            SplitTable.Add("66,p,p,p,p,p,h,h,h,h,h");
            SplitTable.Add("77,p,p,p,p,p,p,h,h,h,h");
            SplitTable.Add("88,p,p,p,p,p,p,p,p,h,h");
            SplitTable.Add("90,p,p,p,p,p,s,p,p,s,s");
            SplitTable.Add("00,s,s,s,s,s,s,s,s,s,s");
            SplitTable.Add("AA,p,p,p,p,p,p,p,p,p,h");
        }

        public bool evaluate(Player a, Player b, bool spl = false)
        {

            int apoints = a.get_points(cards);
            if (spl == true)
                apoints = a.get_points(split_cards);
            if (apoints < 22 && a.counter == 7)
                return true;
            if (apoints > 16)
                return true;
            if (apoints < 9)
                apoints = 8;
            else
                apoints = apoints % 10;
            string y = EvaluationTable[(apoints + 2) % 10];
            char decisions = y[3 + (b.cards[0].value - 2) * 2];
            if (decisions == 's')
                return true;
            else
                return false;
        }

        public bool is_split(Player a, Player b)
        {
            int tmp = a.get_points(a.cards);
            int dp = b.cards[0].value;
            char x, tmp2;
            if (a.cards[0].value == 1 || a.cards[1].value == 1)
                x = 'A';
            else
            {
                x = (char)((tmp / 2) + 48);
            }

            if (x == 'A')
            {
                tmp2 = SplitTable[9][3 + (dp - 2) * 2];
            }
            else
            {
                tmp2 = SplitTable[(tmp / 2 + 8) % 10][3 + (dp - 2) * 2];
            }

            if (tmp2 == 's' || tmp2 == 'h')
                return false;
            else
                return true;
        }

        public bool do_ace(Card x, Player a, Player b, bool spl = false)
        {
            int tmp = a.get_points(a.cards) - 11;
            string y = EvaluationTable[10 + (tmp + 8) % 10];
            char decisions = y[3 + (b.cards[0].value - 2) * 2];
            if (decisions == 's')
                return false;
            else
                a.draw(x, a.counter,spl);
            return false;
        }








    }

}
