using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;






namespace BJ
{


    public partial class Form1 : Form
    {
        public class PLayer
        {
            public List<PictureBox> pic = new List<PictureBox>();
            public List<int> cards = new List<int>();
            public int money = 100;
            public int counter = 2;
            public int Splitcounter = 1;
            public int bet = 0;

            public List<PictureBox> Splitpic = new List<PictureBox>();
            public List<int> Splitcards = new List<int>();

            public int getPoints(List<int> x)
            {
                List<int> bestresult = new List<int>();
                int result = 0;
                for(int i = 0; i < x.Count; i++)
                {
                    if (x[i] % 13 < 7)
                        result += (x[i] % 13 + 2);
                    else if (x[i] % 13 == 7)
                    {
                        result += 9;
                    }
                    else if (x[i] % 13 > 7 && x[i] % 13 < 12)
                    {
                        result += 10;
                    }
                    else if (x[i] % 13 == 12) 
                    {
                        result += 1;
                    }
                }

                bestresult.Add(result);
                for(int i = 0, j=0; i < x.Count; i++)
                {
                    if (x[i] % 13 == 12)
                    {
                        j++;
                        bestresult.Add(result + j * 10);
                    }
                }

                bestresult.Sort();
                int tmp = result;
                for(int i = 0; i < bestresult.Count; i++)
                {
                    if (bestresult[i] > 21)
                    {
                        return tmp;
                    }
                    tmp = bestresult[i];
                }

                return tmp;
                

               
            }
        }


       
        PLayer Human = new PLayer(), Computer = new PLayer(), Dealer = new PLayer();
        public List<PLayer> PLayers = new List<PLayer>();
        List<Image> Cards = new List<Image>();
        List<int> Index = new List<int>();
        Random rnd = new Random(2); //10 //2 //13
        List<string> EvaluationTable = new List<string>();
        List<string> SplitTable = new List<string>();

        void begin()
        {
            int r = rnd.Next(0, Index.Count);
            for(int i = 0; i < 3; i++)
            {
                if (PLayers[i].money < 2 && PLayers[i].bet==0)
                    continue;
                for(int j = 0; j < 2; j++)
                {
                    if (i == 2 && j == 1)
                        continue;
                    //test
                    r = rnd.Next(0, Index.Count);
                    if (i == 1)
                    {
                        r = 13;
                    }
                    if (i == 1 && j == 1)
                        r = 24;
                    //test
                    PLayers[i].cards.Add(Index[r]);
                    PLayers[i].pic[j].Image = Cards[Index[r]];
                    Index.Remove(Index[r]);
                }
            }
          


        }

        void call(bool x = true)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            if (Human.money < 2)
            {
                button4.Enabled = false;
                if (x == false)
                    label5.Text = "koniec gry";
            }
            if (Human.money < 4)
                button5.Enabled = false;
            if (Human.money < 10)
                button6.Enabled = false;
            
        }

        void ComputerCall()
        {
            if (Computer.money > 9)
            {
                Random r = new Random();
                if (r.Next() % 3 == 0)
                {
                    Computer.money -= 10;
                    Computer.bet = 10;
                }
                else if (r.Next() % 3 == 1)
                {
                    Computer.money -= 4;
                    Computer.bet = 4;
                }
                else
                {
                    Computer.money -= 2;
                    Computer.bet = 2;
                }
            }
            else if (Computer.money > 3 && Computer.money < 10)
            {
                Random r = new Random();
                if (r.Next() % 2 == 0)
                {
                    Computer.money -= 4;
                    Computer.bet = 4;
                }
                else
                {
                    Computer.money -= 2;
                    Computer.bet = 2;
                }
            }
            else if (Computer.money < 4 && Computer.money > 2)
            {
                Computer.money -= 2;
                Computer.bet = 2;
            }
            else
            {
                label6.Text = "koniec gry";
            }
        }


        int split()
        {
            int tmp = Computer.getPoints(Computer.cards);
            int dp = Dealer.getPoints(Dealer.cards);
            char x, tmp2;
            if (Computer.cards[0] % 13 == 12 || Computer.cards[1] % 13 == 12)
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
                tmp2 = SplitTable[(tmp/2 + 8) % 10][3 + (dp - 2) * 2];
            }

            if (tmp2 == 's' || tmp2 == 'h')
                return 0;
            else
            {
                Computer.money -= Computer.bet;
                pictureBox29.Image = pictureBox9.Image;
                pictureBox9.Image = null;
                int tm = Computer.cards[0];
                int tm2 = Computer.cards[1];
                Computer.cards.Clear();
                Computer.cards.Add(tm);
                Computer.Splitcards.Add(tm2);
                Computer.counter--;
                label2.Text = Computer.getPoints(Computer.cards).ToString();
                label8.Text += Computer.getPoints(Computer.Splitcards) + "$";
                label6.Text = Computer.money.ToString() + "$";

                int r = rnd.Next(0, Index.Count);
                Computer.pic[Computer.counter].Image = Cards[Index[r]];
                Computer.cards.Add(Index[r]);
                Index.Remove(Index[r]);
                Computer.counter++;

                r = rnd.Next(0, Index.Count);
                Computer.Splitpic[Computer.Splitcounter].Image = Cards[Index[r]];
                Computer.Splitcards.Add(Index[r]);
                Index.Remove(Index[r]);
                Computer.Splitcounter++;

                int cs = 0;
                bool isace = true;
                while ((cs = Computer.getPoints(Computer.Splitcards)) < 22)
                {

                    tmp = cs;
                    bool ace = false;

                    for (int i = 0; i < Computer.Splitcards.Count; i++)
                        if (Computer.Splitcards[i] % 13 == 12)
                        {
                            ace = true;
                            break;
                        }

                    if (ace && isace)
                    {
                        isace = false;
                        int tmp3 = cs - 11;
                        string y = EvaluationTable[10 + (tmp2 + 8) % 10];
                        char decisions = y[3 + (dp - 2) * 2];
                        if (decisions == 's')
                            break;
                        else
                        {
                            r = rnd.Next(0, Index.Count);
                            Computer.Splitpic[Computer.Splitcounter].Image = Cards[Index[r]];
                            Computer.Splitcards.Add(Index[r]);
                            Index.Remove(Index[r]);
                            Computer.Splitcounter++;
                        }

                    }
                    else
                    {
                        if (tmp > 16)
                            break;
                        if (tmp < 9)
                            tmp = 8;
                        else
                            tmp = tmp % 10;
                        string y = EvaluationTable[(tmp + 2) % 10];
                        char decisions = y[3 + (dp - 2) * 2];
                        if (decisions == 's')
                            break;
                        else
                        {
                            r = rnd.Next(0, Index.Count);
                            Computer.Splitpic[Computer.Splitcounter].Image = Cards[Index[r]];
                            Computer.Splitcards.Add(Index[r]);
                            Index.Remove(Index[r]);
                            Computer.Splitcounter++;
                        }
                    }

                }
            }



            return 0;
        }
    

        public Form1()
        {


            //dodanie obrazkow do listy picttureboxow
            int a = 0;
            int b = 0;
            PictureBox tmp = new PictureBox();
            /*
            for (int i = 1; i <= 52; i++)
            {
                //"C: /Users/Piotr/source/repos/Projekt indywidualny/BJ/BJ/Resources/"
                string FilePath = "C: /Users/Piotr/source/repos/Projekt indywidualny/BJ/BJ/Resources/";
                a = i % 10;
                b = (i - a) / 10;
                FilePath += (char)(b + 48);
                FilePath += (char)(a + 48);
                FilePath += ".png";
                tmp.Image = Image.FromFile(FilePath);
                Cards.Add(tmp.Image);
            }
            */

            Cards.Add(BJ.Properties.Resources._01);
            Cards.Add(BJ.Properties.Resources._02);
            Cards.Add(BJ.Properties.Resources._03);
            Cards.Add(BJ.Properties.Resources._04);
            Cards.Add(BJ.Properties.Resources._05);
            Cards.Add(BJ.Properties.Resources._06);
            Cards.Add(BJ.Properties.Resources._07);
            Cards.Add(BJ.Properties.Resources._08);
            Cards.Add(BJ.Properties.Resources._09);
            Cards.Add(BJ.Properties.Resources._10);
            Cards.Add(BJ.Properties.Resources._11);
            Cards.Add(BJ.Properties.Resources._12);
            Cards.Add(BJ.Properties.Resources._13);
            Cards.Add(BJ.Properties.Resources._14);
            Cards.Add(BJ.Properties.Resources._15);
            Cards.Add(BJ.Properties.Resources._16);
            Cards.Add(BJ.Properties.Resources._17);
            Cards.Add(BJ.Properties.Resources._18);
            Cards.Add(BJ.Properties.Resources._19);
            Cards.Add(BJ.Properties.Resources._20);
            Cards.Add(BJ.Properties.Resources._21);
            Cards.Add(BJ.Properties.Resources._22);
            Cards.Add(BJ.Properties.Resources._23);
            Cards.Add(BJ.Properties.Resources._24);
            Cards.Add(BJ.Properties.Resources._25);
            Cards.Add(BJ.Properties.Resources._26);
            Cards.Add(BJ.Properties.Resources._27);
            Cards.Add(BJ.Properties.Resources._28);
            Cards.Add(BJ.Properties.Resources._29);
            Cards.Add(BJ.Properties.Resources._30);
            Cards.Add(BJ.Properties.Resources._31);
            Cards.Add(BJ.Properties.Resources._32);
            Cards.Add(BJ.Properties.Resources._33);
            Cards.Add(BJ.Properties.Resources._34);
            Cards.Add(BJ.Properties.Resources._35);
            Cards.Add(BJ.Properties.Resources._36);
            Cards.Add(BJ.Properties.Resources._37);
            Cards.Add(BJ.Properties.Resources._38);
            Cards.Add(BJ.Properties.Resources._39);
            Cards.Add(BJ.Properties.Resources._40);
            Cards.Add(BJ.Properties.Resources._41);
            Cards.Add(BJ.Properties.Resources._42);
            Cards.Add(BJ.Properties.Resources._43);
            Cards.Add(BJ.Properties.Resources._44);
            Cards.Add(BJ.Properties.Resources._45);
            Cards.Add(BJ.Properties.Resources._46);
            Cards.Add(BJ.Properties.Resources._47);
            Cards.Add(BJ.Properties.Resources._48);
            Cards.Add(BJ.Properties.Resources._49);
            Cards.Add(BJ.Properties.Resources._50);
            Cards.Add(BJ.Properties.Resources._51);
            Cards.Add(BJ.Properties.Resources._52);


            InitializeComponent();

            //przypisanie pictureboxow graczom
            Human.pic.Add(pictureBox1);
            Human.pic.Add(pictureBox2);
            Human.pic.Add(pictureBox3);
            Human.pic.Add(pictureBox4);
            Human.pic.Add(pictureBox5);
            Human.pic.Add(pictureBox6);
            Human.pic.Add(pictureBox7);
            Computer.pic.Add(pictureBox8);
            Computer.pic.Add(pictureBox9);
            Computer.pic.Add(pictureBox10);
            Computer.pic.Add(pictureBox11);
            Computer.pic.Add(pictureBox12);
            Computer.pic.Add(pictureBox13);
            Computer.pic.Add(pictureBox14);
            Dealer.pic.Add(pictureBox15);
            Dealer.pic.Add(pictureBox16);
            Dealer.pic.Add(pictureBox17);
            Dealer.pic.Add(pictureBox18);
            Dealer.pic.Add(pictureBox19);
            Dealer.pic.Add(pictureBox20);
            Dealer.pic.Add(pictureBox21);
            PLayers.Add(Human);
            PLayers.Add(Computer);
            PLayers.Add(Dealer);


            //split
            Human.Splitpic.Add(pictureBox22);
            Human.Splitpic.Add(pictureBox23);
            Human.Splitpic.Add(pictureBox24);
            Human.Splitpic.Add(pictureBox25);
            Human.Splitpic.Add(pictureBox26);
            Human.Splitpic.Add(pictureBox27);
            Human.Splitpic.Add(pictureBox28);
            Computer.Splitpic.Add(pictureBox29);
            Computer.Splitpic.Add(pictureBox30);
            Computer.Splitpic.Add(pictureBox31);
            Computer.Splitpic.Add(pictureBox32);
            Computer.Splitpic.Add(pictureBox33);
            Computer.Splitpic.Add(pictureBox34);
            Computer.Splitpic.Add(pictureBox35);

            //zablokowanie mozliwosci innych guzikow
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;

            for (int i = 0; i < 3; i++)
                PLayers[i].money = 50;

            label1.Text = null;
            label2.Text = null;
            label3.Text = null;
            label4.Text = "krupier dobiera do 16, jak ma wiecej to pasuje";
            label5.Text = Human.money.ToString() + "$";
            label6.Text = Computer.money.ToString() + "$";
            label7.Text = null;
            label8.Text = null;

            //ewaluacja komputera
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

        private void button1_Click(object sender, EventArgs e)
        {

            label7.Text = null;
            label8.Text = null;
            //czyszczenie picturobox
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    PLayers[i].pic[j].Image = null;
                    if (i != 2)
                        PLayers[i].Splitpic[j].Image = null;
                }
            }


            //czyszczenie kard graczy i resetowanie indexu kart
            for(int i = 0; i < 3; i++)
            {
                PLayers[i].cards.Clear();
                PLayers[i].Splitcards.Clear();
                PLayers[i].counter = 2;
                PLayers[i].Splitcounter = 1;
            }
            Dealer.counter = 1;
            Index.Clear();
            for (int i = 0; i < 52; i++)
                Index.Add(i);

            //stawka
            label5.Text = Human.money.ToString() + "$";
            label6.Text = Computer.money.ToString() + "$";
            call();



            //rozdanie dwoch kart graczom
            begin();
       
            //napisanie punktow graczy
            label1.Text = Human.getPoints(Human.cards).ToString();
            label2.Text = Computer.getPoints(Computer.cards).ToString();
            label3.Text = Dealer.getPoints(Dealer.cards).ToString();


            //zablokowanie mozliwosci innych guzikow
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;

            //split

            if (Human.money >= Human.bet && (Human.cards[0] % 13 == Human.cards[1] % 13 || (Human.cards[0] % 13 > 7 && Human.cards[0] % 13 < 12 && Human.cards[1] % 13 > 7 && Human.cards[1] % 13 < 12))) 
            {
                button7.Enabled = true;
            }
            else
                button7.Enabled = false;



        }

        private void button2_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            //dobieranie karty przez gracza
            int r = rnd.Next(0, Index.Count);
            Human.pic[Human.counter].Image = Cards[Index[r]];
            Human.cards.Add(Index[r]);
            Human.getPoints(Human.cards);
            Index.Remove(Index[r]);
            Human.counter++;

            if (Human.getPoints(Human.cards) < 22 && Human.cards.Count == 7)
                label1.Text = "Wygrana" + (2 * Human.bet).ToString() + "$";
            else if (Human.getPoints(Human.cards) < 22)
                label1.Text = Human.getPoints(Human.cards).ToString();
            else
            {
                label1.Text = "Przegrana -" + Human.bet.ToString() + "$";
                button2.Enabled = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int hp = Human.getPoints(Human.cards);
            int hs = Human.getPoints(Human.Splitcards);
            int cp = Computer.getPoints(Computer.cards);
            int cs = Computer.getPoints(Computer.Splitcards);
            int dp = Dealer.getPoints(Dealer.cards);


            //inteligencja komputera
            bool sp = false;
            bool secondturn = true;
            if (Computer.money > 1)
            {
                while ((cp = Computer.getPoints(Computer.cards)) < 22)
                {

                    if (sp == false && (Computer.money >= Computer.bet) && (Computer.cards[0] % 13 == Computer.cards[1] % 13 || (Computer.cards[0] % 13 > 7 && Computer.cards[0] % 13 < 12 && Computer.cards[1] % 13 > 7 && Computer.cards[1] % 13 < 12)))
                    {
                        sp = true;
                        split();
                       // secondturn = false;
                    }
                    else
                    {
                        int tmp = cp;
                        bool ace = false;
                        
                        for (int i = 0; i < Computer.cards.Count; i++)
                            if (Computer.cards[i] % 13 == 12)
                            {
                                ace = true;
                                break;
                            }

                        if (ace && secondturn) 
                        {
                            secondturn = false;
                            int tmp2 = cp - 11;
                            string x = EvaluationTable[10 + (tmp2 + 8) % 10];
                            char decision = x[3 + (dp - 2) * 2];
                            if (decision == 's')
                                break;
                            else
                            {
                                int r = rnd.Next(0, Index.Count);
                                Computer.pic[Computer.counter].Image = Cards[Index[r]];
                                Computer.cards.Add(Index[r]);
                                Index.Remove(Index[r]);
                                Computer.counter++;
                            }

                        }
                        else
                        {
                            secondturn = false;
                            if (tmp > 16)
                                break;
                            if (tmp < 9)
                                tmp = 8;
                            else
                                tmp = tmp % 10;

                        

                            string x = EvaluationTable[(tmp + 2) % 10];
                            char decision = x[3 + (dp - 2) * 2];
                            if (decision == 's')
                                break;
                            else
                            {
                                int r = rnd.Next(0, Index.Count);
                                Computer.pic[Computer.counter].Image = Cards[Index[r]];
                                Computer.cards.Add(Index[r]);
                                Index.Remove(Index[r]);
                                Computer.counter++;
                            }
                        }


                    }

                }       
            }

            while (dp < 17)
            {
                int r = rnd.Next(0, Index.Count);
                Dealer.pic[Dealer.counter].Image = Cards[Index[r]];
                Dealer.cards.Add(Index[r]);
                dp = Dealer.getPoints(Dealer.cards);
                Index.Remove(Index[r]);
                Dealer.counter++;
            }

            //czlowiek
            if ((hp < 22 && dp > 21) || (hp > dp && hp < 22))
            {
                Human.money += 2 * Human.bet;
                label1.Text = "Wygrana + " + (2 * Human.bet).ToString() + "$";
            }
            else if (hp == dp && hp < 22)  {
                Human.money += Human.bet;
                label1.Text = "remis, zwrot " + Human.bet.ToString() + "$";
            }
            else
            {
                label1.Text = "Przegrana - " + Human.bet.ToString() + "$";
            }


            if (hs == 0)
            {
                label7.Text = null;
            }
            else if  ((hs < 22 && dp > 21) || (hs > dp && hs < 22)) 
            {
                Human.money += 2 * Human.bet;
                label7.Text = "Wygrana + " + (2 * Human.bet).ToString() + "$";
            }
            else if (hs == dp && hs < 22)
            {
                Human.money += Human.bet;
                label7.Text = "remis, zwrot " + Human.bet.ToString() + "$";
            }
            else
            {
                label7.Text = "Przegrana - " + Human.bet.ToString() + "$";
            }

            //komputer
            if ((cp < 22 && dp > 21) || (cp > dp && cp < 22))
            {
                Computer.money += 2 * Computer.bet;
                label2.Text = "Wygrana + " + (2 * Computer.bet).ToString() + "$";
            }
            else if (cp == dp && cp < 22) 
            {
                Computer.money += Computer.bet;
                label2.Text = "remis, zwrot " + Human.bet.ToString() + "$";
            }
            else
            {
                label2.Text = "Przegrana - " + Computer.bet.ToString() + "$";
            }

            cs = Computer.getPoints(Computer.Splitcards);

            if (cs == 0)
            {
                label7.Text = null;
            }
            else if ((cs < 22 && dp > 21) || (cs > dp && cp < 22))
            {
                Computer.money += 2 * Computer.bet;
                label8.Text = "Wygrana + " + (2 * Computer.bet).ToString() + "$";
            }
            else if (cs == dp && cp < 22)
            {
                Computer.money += Computer.bet;
                label8.Text = "remis, zwrot " + Human.bet.ToString() + "$";
            }
            else
            {
                label8.Text = "Przegrana - " + Computer.bet.ToString() + "$";
            }


            label5.Text = Human.money.ToString() + "$";
            label6.Text = Computer.money.ToString() + "$";
            label3.Text = Dealer.getPoints(Dealer.cards).ToString();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
            call(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Human.money -= 2;
            Human.bet = 2;
            button1.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            ComputerCall();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            Human.money -= 4;
            Human.bet = 4;
            ComputerCall();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            Human.money -= 10;
            Human.bet = 10;
            ComputerCall();
            //Computer.money -= 91;
        }


        private void button7_Click(object sender, EventArgs e)
        {
            Human.money -= Human.bet;
            label7.Text = "SPLIT ";
            pictureBox22.Image = pictureBox2.Image;
            pictureBox2.Image = null;
            int tmp = Human.cards[0];
            int tmp2 = Human.cards[1];
            Human.cards.Clear();
            Human.cards.Add(tmp);
            Human.Splitcards.Add(tmp2);
            Human.counter--;
            label1.Text = Human.getPoints(Human.cards).ToString();
            label7.Text += Human.getPoints(Human.Splitcards) + "$";
            label5.Text = Human.money.ToString() + "$";
            button7.Enabled = false;
            button8.Enabled = true;
            button9.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int r = rnd.Next(0, Index.Count);
            Human.Splitpic[Human.Splitcounter].Image = Cards[Index[r]];
            Human.Splitcards.Add(Index[r]);
            Index.Remove(Index[r]);
            Human.Splitcounter++;
            int tmp = Human.getPoints(Human.Splitcards);

            if (tmp < 22 && Human.Splitcards.Count == 7)
                label7.Text = "Wygrana" + (2 * Human.bet).ToString() + "$";
            else if (tmp < 22)
                label7.Text = tmp.ToString();
            else
            {
                label7.Text = "Przegrana -" + Human.bet.ToString() + "$";
                button2.Enabled = true;
                button3.Enabled = true;
                button8.Enabled = false;
                button9.Enabled = false;
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button8.Enabled = false;
            button9.Enabled = false;
        }







































        private void pictureBox1_Click(object sender, EventArgs e)
        {
      
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
      
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

    
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
