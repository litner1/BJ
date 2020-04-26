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

        public static bool newgame = false;


        List<Card> Cards = new List<Card>();
        List<Card> tmp_cards = new List<Card>();
        Player Human = new Player(), Computer = new Player(), Dealer = new Player();
        List<Player> Players = new List<Player>();
        Random rnd = new Random();//2
        List<Button> buttons = new List<Button>();


        void setbox()
        {
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

            //split
            Human.split_pic.Add(pictureBox22);
            Human.split_pic.Add(pictureBox23);
            Human.split_pic.Add(pictureBox24);
            Human.split_pic.Add(pictureBox25);
            Human.split_pic.Add(pictureBox26);
            Human.split_pic.Add(pictureBox27);
            Human.split_pic.Add(pictureBox28);
            Computer.split_pic.Add(pictureBox29);
            Computer.split_pic.Add(pictureBox30);
            Computer.split_pic.Add(pictureBox31);
            Computer.split_pic.Add(pictureBox32);
            Computer.split_pic.Add(pictureBox33);
            Computer.split_pic.Add(pictureBox34);
            Computer.split_pic.Add(pictureBox35);

        }

        void do_split()
        {
            Computer.money -= Computer.bet;
            pictureBox29.Image = pictureBox9.Image;
            pictureBox9.Image = null;
            Card tm = Computer.cards[0];
            Card tm2 = Computer.cards[1];
            Computer.cards.Clear();
            Computer.cards.Add(tm);
            Computer.split_cards.Add(tm2);
            Computer.counter--;

            int r = rnd.Next(tmp_cards.Count);
            Computer.draw(tmp_cards[r], Computer.split_counter, true);
            tmp_cards.Remove(tmp_cards[r]);
            Computer.split_counter++;

            r = rnd.Next(tmp_cards.Count);
            Computer.draw(tmp_cards[r], Computer.counter);
            tmp_cards.Remove(tmp_cards[r]);
            Computer.counter++;

            label2.Text = "Kapitał Gracza Komputerowego " + Computer.money.ToString() + "$" + "\nStawka " + Computer.bet.ToString() + "$   " + Computer.get_points(Computer.cards).ToString() + "pkt";
            label9.Text = "Split   " + Computer.get_points(Computer.split_cards).ToString() + "pkt";

            //dzialanie
            if (Computer.split_cards[0].value == 1 || Computer.split_cards[1].value == 1)
            {
                r = rnd.Next(tmp_cards.Count);
                Computer.do_ace(tmp_cards[r], Computer, Dealer,true);
            }

            while (Computer.get_points(Computer.split_cards) < 22)
            {
                if (Computer.evaluate(Computer, Dealer, true) == true)
                {
                    label9.Text = "Split   " + Computer.get_points(Computer.split_cards).ToString() + "pkt";
                    break;
                }
                r = rnd.Next(tmp_cards.Count);
                Computer.draw(tmp_cards[r], Computer.split_counter,true);
                tmp_cards.Remove(tmp_cards[r]);
                Computer.split_counter++;
                label9.Text = "Split   " + Computer.get_points(Computer.split_cards).ToString() + "pkt";
                if (Computer.get_points(Computer.cards) > 21)
                    label2.Text += " FURA";
            }


        }

        void do_color()
        {
            int[] enable = { 50, 205, 50 };
            int[] disable = { 220, 20, 60 };

            for (int i = 0; i < buttons.Count(); i++)
                if (buttons[i].Enabled == true)
                    buttons[i].BackColor = Color.FromArgb(enable[0],enable[1],enable[2]);
                else
                    buttons[i].BackColor = Color.FromArgb(disable[0],disable[1],disable[2]);

            if (textBox1.Enabled == true)
            {
                textBox1.BackColor = Color.FromArgb(enable[0], enable[1], enable[2]);
                label6.BackColor = Color.FromArgb(enable[0], enable[1], enable[2]);
            }
            else
            {
                textBox1.BackColor = Color.FromArgb(disable[0], disable[1], disable[2]);
                label6.BackColor = Color.FromArgb(disable[0], disable[1], disable[2]);
            }


        }

        public Form1() {

            //dodanie obrazkow kart
            int a = 0;
            int b = 0;
            for (int i = 1; i <= 52; i++)
            {
                Card tmp = new Card();
                string Path = System.IO.Path.GetFullPath(@"..\..\") + "Resources\\";
                Path = Path.Replace('\\', '/');
                a = i % 10;
                b = (i - a) / 10;
                Path += (char)(b + 48);
                Path += (char)(a + 48);
                Path += ".png";
                tmp.image = Image.FromFile(Path);
                tmp.reverse = false;
                tmp.value = tmp.get_value(b * 10 + a);
                Cards.Add(tmp);
            }

            InitializeComponent();

            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            buttons.Add(button7);
            buttons.Add(button8);
            buttons.Add(button9);
            buttons.Add(button10);

            Computer.ev();
            Players.Add(Human);
            Players.Add(Computer);
            Players.Add(Dealer);
            Human.money = 50;
            Computer.money = 50;
            Dealer.money = 50;
            setbox();

            label1.Text = "Mój kapitał 50$";
            label2.Text = "Kapitał Gracza Komputerowego 50$";
            label3.Text = "Krupier\n";
            label7.Text = null;
            label8.Text = null;
            label9.Text = null;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button9.Enabled = false;

            do_color();
            newgame = false;
        }

   
        private void button1_Click(object sender, EventArgs e)
        {
            //kopiowanie kart
            tmp_cards.Clear();
            for (int i = 0; i < 52; i++)
            {
                Card x = Cards[i];
                tmp_cards.Add(x);
            }

            //rozdanie kart
            int r;
            for(int i = 0; i < 3; i++)
            {
                if (Players[i].money < 1 && Players[i].bet==0)
                    continue;
                for(int j = 0; j < 2; j++)
                {

                    r = rnd.Next(tmp_cards.Count);

                   //test
                    /*
                    if (i == 1)
                        r = 4;
                    if (i == 1 && j == 1)
                        r = 15;
                   // if(i==2 && j==1)
                       // r = 4;
                    */

                    Players[i].cards.Add(tmp_cards[r]);
                    tmp_cards.Remove(tmp_cards[r]);
                    Players[i].pic[j].Image = Players[i].cards[j].image;
                }
            }
            //ukrycie kart
            if (Computer.money > 0 || Computer.bet > 0)
            {
                Computer.cards[0].reverse_image = Computer.cards[0].image;
                Computer.cards[1].reverse_image = Computer.cards[0].image;
                Computer.cards[0].reverse = true;
                Computer.cards[1].reverse = true;
                pictureBox8.Image = BJ.Properties.Resources.rewers;
                pictureBox9.Image = BJ.Properties.Resources.rewers;
            }
            Dealer.cards[1].reverse_image = Dealer.cards[0].image;
            pictureBox16.Image = BJ.Properties.Resources.rewers;
            Dealer.cards[1].reverse = true;

            //porzadki
            label1.Text += "   " + Human.get_points(Human.cards).ToString() + "pkt";
            if (Computer.money > 0 || Computer.bet > 0 || Computer.bet > 0)
                label2.Text = "Kapitał Gracza Komputerowego " + Computer.money.ToString() + "$" + "\nStawka " + Computer.bet.ToString() + "$   " + Computer.get_points(Computer.cards).ToString() + "pkt";
            else
                label2.Text = null;
            label3.Text += Dealer.get_points(Dealer.cards).ToString()+"pkt";
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            if (Human.money >= Human.bet)
                button9.Enabled = true;

            if (Human.cards[0].value == Human.cards[1].value && Human.money>=Human.bet)
                button4.Enabled = true;

            do_color();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //dobranie karty
            int r = rnd.Next(tmp_cards.Count);
            Human.draw(tmp_cards[r],Human.counter);
            tmp_cards.Remove(tmp_cards[r]);
            Human.counter++;
            label1.Text = "Mój kapitał " + Human.money.ToString() + "$" + "\nStawka " + Human.bet.ToString() + "$   " + Human.get_points(Human.cards).ToString() + "pkt";
            if (Human.get_points(Human.cards)<22 && Human.counter == 7)
            {
                label1.Text += " Wygrana";
                button2.Enabled = false;
            }

            if (Human.get_points(Human.cards) > 21)
            {
                label1.Text += " FURA";
                button2.Enabled = false;
            }
            button4.Enabled = false;
            button9.Enabled = false;

            do_color();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //odkrycie kart
            if (Computer.money > 0 || Computer.bet > 0)
            {
                Computer.cards[0].reverse = false;
                Computer.cards[1].reverse = false;
                pictureBox8.Image = Computer.cards[0].image;
                pictureBox9.Image = Computer.cards[1].image;
            }
            Dealer.cards[1].reverse = false;
            pictureBox16.Image = Dealer.cards[1].image;

        
            //skroty
            int hp = Human.get_points(Human.cards);
            int cp = Computer.get_points(Computer.cards);
            int dp = Dealer.get_points(Dealer.cards);
            int hs = Human.get_points(Human.split_cards);
            int cs = Computer.get_points(Computer.split_cards);


            //komputer dobiera karty
            if (Computer.money > 0 || Computer.bet > 0)
            {
                bool split = false;
                if (Computer.cards[0].value == Computer.cards[1].value && Computer.money >= Computer.bet && Computer.get_points(Computer.split_cards) == 0)
                    split = Computer.is_split(Computer, Dealer);
                if (split)
                    do_split();

                if (Computer.cards[0].value == 1 || Computer.cards[1].value == 1)
                {
                    int r = rnd.Next(tmp_cards.Count);
                    Computer.do_ace(tmp_cards[r], Computer, Dealer);
                }

                while ((cp = Computer.get_points(Computer.cards)) < 22)
                {
                    if (Computer.evaluate(Computer,Dealer))
                        break;

                    int r = rnd.Next(tmp_cards.Count);
                    Computer.draw(tmp_cards[r], Computer.counter);
                    tmp_cards.Remove(tmp_cards[r]);
                    Computer.counter++;
                }
            }

            label2.Text = "Kapitał Gracza Komputerowego " + Computer.money.ToString() + "$" + "\nStawka " + Computer.bet.ToString() + "$   " + Computer.get_points(Computer.cards).ToString() + "pkt";
            if (Computer.get_points(Computer.cards) > 21)
                label2.Text += " FURA";

            //dealer dobiera karty
            while ((dp = Dealer.get_points(Dealer.cards)) < 17) 
            {
                int r = rnd.Next(tmp_cards.Count);
                Dealer.draw(tmp_cards[r], Dealer.counter);
                tmp_cards.Remove(tmp_cards[r]);
                Dealer.counter++;
            }
            label3.Text = "Krupier\n" + Dealer.get_points(Dealer.cards).ToString()+"pkt";

            //czlowiek

            X tmp = Dealer.Results(Human, Dealer, true);
            if (tmp == X.w)
                label8.Text += "\nWygrana +" + (2 * Human.bet).ToString() + "$";
            else if (tmp == X.r)
                label8.Text += "\nRemis, zwrot " + Human.bet.ToString() + "$";
            else if (tmp == X.p)
                label8.Text += "\nPrzegrana -" + Human.bet.ToString() + "$";
            else
                label8.Text = null;

            tmp = Dealer.Results(Human, Dealer);
            label1.Text = "Mój kapitał " + Human.money.ToString() + "$" + "\nStawka " + Human.bet.ToString() + "$   " + Human.get_points(Human.cards).ToString() + "pkt";

            if (tmp == X.w)
                label1.Text += "\nWygrana +" + (2 * Human.bet).ToString() + "$";
            else if (tmp == X.r)
                label1.Text += "\nRemis, zwrot " + Human.bet.ToString() + "$";
            else if (tmp == X.p)
                label1.Text += "\nPrzegrana -" + Human.bet.ToString() + "$";
            else
                label1.Text = null;





            //komputer
            tmp = Dealer.Results(Computer, Dealer, true);
            if (tmp == X.w)
                label9.Text += "\nWygrana +" + (2 * Computer.bet).ToString() + "$";
            else if (tmp == X.r)
                label9.Text += "\nRemis, zwrot " + Computer.bet.ToString() + "$";
            else if (tmp == X.p)
                label9.Text += "\nPrzegrana -" + Computer.bet.ToString() + "$";
            else
                label9.Text = null;

            tmp = Dealer.Results(Computer, Dealer);
            if (tmp == X.w)
                label2.Text += "\nWygrana +" + (2 * Computer.bet).ToString() + "$";
            else if (tmp == X.r)
                label2.Text += "\nRemis, zwrot " + Computer.bet.ToString() + "$";
            else if (tmp == X.p)
                label2.Text += "\nPrzegrana -" + Computer.bet.ToString() + "$";
            else
                label2.Text = null;


            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;
            if (Human.money < 1)
            {
                button7.Enabled = true;
                label1.Text = "KONIEC GRY";
            }
            else
            {
                textBox1.Clear();
                textBox1.Enabled = true;
                button8.Enabled = true;
            }

            do_color();
        }




        private void button4_Click(object sender, EventArgs e)
        {
            Human.money -= Human.bet;
            pictureBox22.Image = pictureBox2.Image;
            pictureBox2.Image = null;
            Card tmp = Human.cards[0];
            Card tmp2 = Human.cards[1];
            Human.cards.Clear();
            Human.cards.Add(tmp);
            Human.split_cards.Add(tmp2);
            Human.counter--;
            label1.Text = "Mój kapitał " + Human.money.ToString() + "$" + "\nStawka " + Human.bet.ToString() + "$   " + Human.get_points(Human.cards).ToString() + "pkt";
            label8.Text = "Split   " + Human.get_points(Human.split_cards).ToString() + "pkt";
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button9.Enabled = false;

            do_color();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int r = rnd.Next(tmp_cards.Count);
            Human.draw(tmp_cards[r], Human.split_counter,true);
            tmp_cards.Remove(tmp_cards[r]);
            Human.split_counter++;
            label8.Text = "Split   " + Human.get_points(Human.split_cards).ToString()+"pkt";
            if (Human.get_points(Human.split_cards) < 22 && Human.split_counter == 7)
            {
                label1.Text += " \nWygrana";
                button2.Enabled = false;
            }
            if (Human.get_points(Human.split_cards) > 21)
            {
                label8.Text += " FURA";
                button5.Enabled = false;
            }

            do_color();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            do_color();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            if(button7.Text=="NOWA GRA")
            {
                newgame = true;
                this.Close();
            }

            label1.Text = null;
            label2.Text = null;
            label3.Text = null;
            label6.Text = null;
            label7.Text = "       Dziękuję za grę";
            label7.Font = new Font("Calibri", 40);
            label8.Text = null;
            label9.Text = null;
            textBox1.Text = null;
            for (int i = 0; i < 3; i++)
            {
                Players[i].cards.Clear();
                Players[i].split_cards.Clear();
                Players[i].counter = 2;
                Players[i].split_counter = 1;

                for (int j = 0; j < 7; j++)
                {
                    Players[i].pic[j].Image = null;
                    if (i == 2)
                        continue;
                    Players[i].split_pic[j].Image = null;
                }

            }
            button7.Text="NOWA GRA";
            do_color();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && Int32.Parse(textBox1.Text) <= Human.money && Int32.Parse(textBox1.Text) > 0)
            {
                //czyszczenie pictureboxow
                for (int i = 0; i < 3; i++)
                {
                    Players[i].cards.Clear();
                    Players[i].split_cards.Clear();
                    Players[i].counter = 2;
                    Players[i].split_counter = 1;
                    Players[i].bet = 0;

                    for (int j = 0; j < 7; j++)
                    {
                        Players[i].pic[j].Image = null;
                        if (i == 2)
                            continue;
                        Players[i].split_pic[j].Image = null;
                    }

                }

                //stawki
                Human.bet = Int32.Parse(textBox1.Text);
                Human.money -= Human.bet;
                label1.Text = "Mój kapitał " + Human.money.ToString() + "$" + "\nStawka " + Human.bet.ToString() + "$";
                if (Computer.money > 0 || Computer.bet > 0)
                {
                    Computer.bet = rnd.Next(1, (Computer.money + 1) / 2);
                   // Computer.bet = Computer.money;
                    Computer.money -= Computer.bet;
                    label2.Text = "Kapitał Gracza Komputerowego " + Computer.money.ToString() + "$" + "\nStawka " + Computer.bet.ToString() + "$";
                }
                else
                {
                    Computer.bet = 0;
                    label2.Text = null;
                }
                label3.Text = "Krupier\n";

           
                label8.Text = null;
                label9.Text = null;
                textBox1.Enabled = false;
                button1.Enabled = true;
                button8.Enabled = false;
            }
            else
                MessageBox.Show("nie mozesz zagrac za tyle $");
            do_color();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            /*
            string tmp = (System.IO.Path.GetFullPath(@"..\..\") + "rules.txt").Replace('\\', '/');
            MessageBox.Show(File.ReadAllText(tmp, Encoding.GetEncoding(1250)));
            */
            MessageBox.Show(File.ReadAllText((System.IO.Path.GetFullPath(@"..\..\") + "rules.txt").Replace('\\', '/'), Encoding.GetEncoding(1250)));
        }



        private void button9_Click(object sender, EventArgs e)
        {
            Human.money -= Human.bet;
            Human.bet *= 2;
            label1.Text = "Mój kapitał " + Human.money.ToString() + "$" + "\nStawka " + Human.bet.ToString() + "$   " + Human.get_points(Human.cards).ToString() + "pkt";
            button9.Enabled = false;
            button4.Enabled = false;
            do_color();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //nie pozwalamy na nic innego niz liczby
            char x = e.KeyChar;
            if (!Char.IsDigit(x) && !(x=='\r' || x==8))
            {
                e.Handled = true;
                MessageBox.Show("nieprawidlowa warotsc");
            }
           
        }


    }
}
