using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace JADE
{
    /// <summary>
    ///Form-Objekt(Windows Fenster) für das Hauptfenster unserer Anwendung.
    /// </summary>
    public partial class HauptFenster : Form
    {
        /**Statische Variable vom Typ Daten. Erhält die Daten der TinySegmenter Funktion. */
        public static Daten Instanzdaten;
        /**Statische Variable vom Typ SearchEngine. Wird im HauptFenster für den Aufruf der Suchfunktion benötigt. */
        private static SearchEngine suche;
        /**Statische Variable vom Typ Segmenter. Wird im HauptFenster für den Aufruf der TinySegmenter-Funktion benötigt. */
        private static Segmenter segtest;

        /**
         * Konstruktor für das HauptFenster.
         * Initialisiert die nötigen Komponenten.
         */     
        public HauptFenster()
        {
            InitializeComponent();
            this.Icon = new System.Drawing.Icon("jade.ico");

            flowLayoutPanel_Token.HorizontalScroll.Enabled = false;
            flowLayoutPanel_Token.HorizontalScroll.Visible = false;
            Instanzdaten = new Daten();
            suche = SearchEngine.Engine;
            segtest = new Segmenter();
        }

        /**
        * Diese Funktion passt bei einem Klick auf eine Zelle in der Tabelle für die Suchergebnisse die Größe der Tabellenspalten an die Inhalte der angezeigten Zeilen an.
        */
        private void event_dataGridView_Click(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView_Suchergebnisse.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);   //Automatische Spaltenskalierung.
        }

        /**
        * Diese Funktion sorgt dafür, dass nach dem Klicken auf einen Satz das FlowLayoutPanel aktualisiert wird und damit die richtigen Token angezeigt werden.
        */
        public void event_TreeViewItemSelect(object sender, TreeViewEventArgs e)
        {
            flowupdate();                                                                           //Ruft Funktion flowupdate auf.
        }

         /**
         * Diese Funktion dient der Aktualisierung der Bereiche, in dem die Token eines Satzes angezeigt bzw. über Checkboxen ausgewählt werden können.
         * Die Anzeige geschieht immer für den in der Treeview gerade ausgewählten Satz des eingegebenen Textes.
         */
        public void flowupdate()
        {   
            TreeNode selectedTreeNode = treeView_Sätze.SelectedNode;                                //Erstellt TreeNodeobjekt.
            int satzIndex = this.treeView_Sätze.SelectedNode.Index;                                 //Gibt Verweis auf aktuellen TreeNodeobjekt.
            
            if (selectedTreeNode != null)                                                           //Wenn Treeview Saetze enhaelt, werden diese Eintraege hier geloescht.
            {
                this.flowLayoutPanel_Token.Controls.Clear();                                        //Loescht alle Checkboxen aus dem Flowlayoutpannel.
                string satzText = "";                                                               //Erstellt leeren String.
                List<Control> checkBoxes = new List<Control>();                                     //Erstellt ein Control fuer Checkboxes.
                ArrayList satz = Instanzdaten.getSatz(satzIndex);                                   //Erstellt eine Arraylist und erhaelt den Satz der markiert wurde.
                int i = 0;                                                                          //Erstellt Hilfsvariable.
                string token = "";                                                                  //Erstellt Hilfsvariable.
                    
                foreach (var tokenObj in satz)                                                      //Jedes Token im Satz wird mit Leerzeichen an string angehangen.
                {
                    token = (String)tokenObj;
                    satzText += token + "  ";
                    
                    if (!("、？！。".Contains(token)))                                              //Falls "?", ",", "!" oder "." enhalten wird keine Checkbox fuer dieser erstellt.
                    {
                        checkBoxes.Add
                            (new System.Windows.Forms.CheckBox                                      //Fuer Jedes Token wird neue Checkbox erstellt.
                            { 
                                Name = "" + i,
                                Text = token,
                                AutoSize = true                                                     //Autosize ist an.
                            });
                    }
                    i++;
                }

                this.textBox1.Text = satzText;                                                      //Uebergibt der Textbox den String mit dem Token.
                this.flowLayoutPanel_Token.Controls.AddRange(checkBoxes.ToArray());                 //Uebergibt die Checkboxen an ein Array und dann and das FlowLayoutPanel.
            }
        }

        /**
         * EventFunktion: Beim Klick auf den Tokenize-Button wird der Text aus der Richtextbox tokenisiert und in einer Instanz unserer Daten-Klasse (Variable Instanzdaten) festgehalten.
         * Zunächst wird der Text aus der Richtextbox von störenden Whitespace-Chars bereinigt.
         * Danach wird geprüft, ob sich nicht-japanische Zeichen im Text befinden und gegebenenfalls eine Warnmeldung ausgegeben.
         * Sofern ein Text vorhanden ist, wird dieser mit der TinySegmenter-Funktion tokenisiert.
         * Danach werden die Daten ausgelesen und die vorhandenen Sätze in der Treeview repräsentiert.
         */
        private void Tokenize_Click(object sender, EventArgs e)
        {
            String toTok = this.richTextBox1.Text;                                                  //Fuegt Text aus Richtextbox in ein String.
            toTok = toTok.Replace("\n", "");                                                        //Loescht alle Zeilvorschübe.
            toTok = toTok.Replace("\t", "");                                                        //Loescht alle Whitespacecharakter.
            toTok = toTok.Replace(" ", "");                                                         //Loescht Leerzeichen.
            toTok = toTok.Replace("　", "");                                                        //Loescht japanische Leerzeichen.
            this.richTextBox1.Text = toTok;                                                         //Fuegt Text von Richtextbox erneunt an String an.
            segtest.TextTest(this.richTextBox1.Text);
            
            if (!(Equals(richTextBox1.Text, "")))                                                   //Wenn die Textbox nicht leer ist, läuft Programm weiter.
            {
                suche.clearDataSet();
                dataGridView_Suchergebnisse.DataSource = null;
                this.treeView_Sätze.Nodes.Clear();
                this.textBox1.Text = "";
                this.flowLayoutPanel_Token.Controls.Clear();

                Instanzdaten = segtest.TinySegmenter(toTok);                                        //Weißt dem Objekt die Daten aus der richTextBox zu
                ArrayList Alist = Instanzdaten.Zugriff;                                             //Erstellt eine Arraylist.

                int i = 0;
                foreach (ArrayList a in Alist)                                                      //Für Jeden Eintrag im Array wird ein neuer Eintrag im Treeview erstellt.
                {
                    TreeNode Instanz = new TreeNode("Satz " +  ("" + (i + 1)).PadLeft(2,'0') + ": " + Instanzdaten.getToken(i, 0));
                    treeView_Sätze.Nodes.Add(Instanz);
                    i++;
                }
            }
            else                                                                                    //Falls Textbox leer ist, wird eine Fehlermeldung ausgegeben.
            {
                MessageBox.Show("Kein Text zum Tokenisieren gegeben", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        /**
         * Diese Funktion setzt das Trennen eines Tokens um und ruft dazu das BeabeitenFenster auf.
         * @param[in] Satznummer int-Wert (Indexwert für Zugriff auf entsprechende Arraylists in Instanzdaten) des Satzes, in dem sich das zu ändernde Token befindet.
         * @param[in] Tok int-Wert (Indexwert für Zugriff auf entsprechende Arraylists in Instanzdaten) des Tokens, das aufgeteilt werden soll.
         */
        public void trennen(int Satznummer, int Tok)
        {
            String str = Instanzdaten.getToken(Satznummer, Tok);
            BearbeitenFenster neumit = new BearbeitenFenster(Instanzdaten, Satznummer, Tok, str);           //Erzeugt Bearbeiten-Fenster.
            neumit.FormClosing += new FormClosingEventHandler(event_BearbeitenFensterSchliessen);           //Erzeugt für das neue Fenster einen Fenster schließen Eventhandler.
            neumit.Show();                                                                                  //Zeigt das Bearbeiten-Fenster.
        }

        /**
         * EventHandler: Der beim Schließen des Bearbeiten-Fensters, den Listener ausschaltet und das FlowlayoutPanel mit den Token (die Anzeige der Tokenliste) updatet.
         */
        void event_BearbeitenFensterSchliessen(object sender, FormClosingEventArgs e)
        {
            BearbeitenFenster neumit = (BearbeitenFenster)sender;
            neumit.FormClosing -= new FormClosingEventHandler(event_BearbeitenFensterSchliessen);
            flowupdate();
        }

        /**
         * Diese Funktion realisiert das Zusammenfügen zweier nebeneinanderliegender, ausgewählter Token.
         * Sie ist noch ausgelegt auf kleine Textgrössen und muss noch angepasst werden, um das Auslesen aller Daten für ein Zusammenfügen von Token zu verhindern.
         * Außerdem werden noch zwei int-Werte (Indexwerte für Zugriff auf entsprechende Arraylists in Instanzdaten) übergeben welche den Zugriff auf den entsprechenden Satz und die betroffenen Token möglich machen.
         * @param[in] Satznummer int-Wert (Indexwert für Zugriff auf entsprechende Arraylists in Instanzdaten) des Satzes in dem sich der zu ändernde Token befindet.
         * @param[in] Tok1 int-Wert (Indexwert) des ersten Tokens, das mit seinem Nachfolger-Token zusammengefügt werden soll.
         * @param[in] Tok2 int-Wert (Indexwert) des zweiten Tokens, das mit seinem Vörgänger-Token zusammengefügt werden soll.
         */
        public void zusammen(int Satznummer, int Tok1, int Tok2)
        {
            ArrayList Alist = Instanzdaten.Zugriff;                                                         //Zugriff auf Datenstruktur.
            ArrayList Satz = Instanzdaten.getSatz(Satznummer);                                              //Heraussuchen des Satzes in welchem sich das zu ändernde Token befindet.   
            Satz[Tok1] = ((String)Satz[Tok1] + (String)Satz[Tok2]);                                         //Zusammenfügen der Token tok1 und tok2 an Position von tok1.
            Satz.RemoveAt(Tok2);                                                                            //Loeschen des nun ueberflüssigen tok2.
            Alist[Satznummer] = Satz;                                                                       //Schreiben des geänderten Satzes in die Arraylist.
            Instanzdaten.Zugriff = Alist;                                                                   //Schreiben der geänderten ARRAYLIST zurück in die Datenstruktur.

            //Aktualisieren des dataSet der SearchEngine
            SearchEngine.DisposeTable(Satznummer, Tok1);
            SearchEngine.DisposeTable(Satznummer, Tok2);

            for(int i = Tok1 + 2; i < Satz.Count; i++)
            {
                SearchEngine.ShiftTable(Satznummer, i, true);
            }
        }

        /**EventFunktion welche beim klick auf den Öffnen-Dialog im Menü aufgerufen wird.
         * File-Open Dialog der das Auswählen einer Textdatei ermöglicht, welche dann gelesen und in die RichTextBox des Hauptfensters geschrieben wird.
         */ 
        private void öffnenToolStripMenu_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.RestoreDirectory = true;

            StreamReader openFileStream;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((openFileStream = new  StreamReader(openFileDialog1.FileName)) != null)
                    {
                        using (openFileStream)
                        {
                            richTextBox1.Text = openFileStream.ReadToEnd();
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Es ist ein Fehler beim Öffnen der angegebenen Datei aufgetreten.\n\nFehlermeldung:\n" + exception.Message, "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                }
            }

        }

        /**EventFunktion: Bei klick auf den "Token bearbeiten"-Button.
         * Überprüft zuerst ob eine korrekte Auswahl an Token (makierte checkBoxen in der FlowLayoutPanel) vorliegen. Bei fehlerhafter Auswahl erfolgt Warnmeldung.
         * Sofern korrekte Auswahl vorliegt, wird unterschieden zwischen: 
         * - 1 Token makiert --> Token trennen Funktion wird auf dem ausgewählten Token ausgeführt.
         * - 2 Token makiert --> Token zusammen Funktion wird auf den ausgewählten Token ausgeführt.
         */
        private void TokenBearbeiten_Click(object sender, EventArgs e)
        {
            int count = 0;                                                          //Variable "count" wird eingeführt.
            int first = 0;                                                          //Variable "first" wird eingeführt.
            int second = 0;                                                         //Variable "second" wird eingeführt.
            foreach (Control con in this.flowLayoutPanel_Token.Controls)            //Zaehlt wie viel Checkboxen ausgewählt wurden.
            {
                CheckBox box = (CheckBox)con;
                int index = Int32.Parse(box.Name); 
                if (box.Checked == true)
                {
                    if (count == 0)
                    {
                        first = index;
                    }
                    if (count > 0)
                    {
                        second = index;
                    }
                    count++;
                }
            }
            if (count > 2)                                                          //Fehlerausgabe wenn mehr als 2 Checkboxen ausgewaehlt wurden.
            {
                MessageBox.Show("Bitte Maximal 2 (nebeneinander liegende) Token auswählen.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                switch (count)
                {
                    case 1:                                                         //Trennt die ausgewaehlten Token.
                        trennen(this.treeView_Sätze.SelectedNode.Index, first);
                        break;
                    case 2:
                        if (first + 1 != second)                                    //Fehlerausgabe wenn Checkboxen nicht nebeneinader liegen.
                        {
                            MessageBox.Show("Bitte Maximal 2 (nebeneinander liegende) Token auswählen.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        else                                                        //Fuegt Token zusammen.
                        {
                            zusammen(this.treeView_Sätze.SelectedNode.Index, first, second);
                            flowupdate();                                           //Ruft flowupdate-Funktion auf.
                            break;                                                  //beendet diese Funktion.
                        }
                }
            }
        }

        /**EventFunktion: Bei klick auf den "Token suchen"-Button.
         * Überprüft zuerst ob eine korrekte Auswahl an Token (makierte checkBoxen in der FlowLayoutPanel) vorliegen. Bei fehlerhafter Auswahl erfolgt Fehlerhinweis.
         * Sofern richtige Auswahl vorliegt werden die vorliegenden Informationen (gewählter Token, detailierte Suche ja/nein) an die Suchfunktion search übergeben, welche eine Ergebnissliste(DataTable) erstellt und diese im GridView abbildet. 
         */
        private void TokenSuchen_Click(object sender, EventArgs e)
        {
            int count = 0;                                                                  //Erzeugt die Variable "Count" mit dem Wert Null.
            int first = 0;                                                                  //Erzeugt die Variable "first" mit dem Wert Null.
            foreach (Control con in this.flowLayoutPanel_Token.Controls)                         
            {
                CheckBox box = (CheckBox)con;
                int index = Int32.Parse(box.Name);
                if (box.Checked == true)
                {
                    if (count == 0)
                    {
                        first = index;
                    }
                    count++;
                }
            }
            if (count > 1)                                                                  //Eventhaendler, wenn mehr als eine Checkbox ausgewaehlt ist, wird eine Fehlermeldung ausgegeben.
            {
                MessageBox.Show("Bitte wählen Sie maximal ein Token aus.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (count == 1)                                                                 //Eventhaendler, wenn genau eine Checkbox ausgewaehlt ist.
            {
                DataTable result = suche.search(Instanzdaten.getToken(this.treeView_Sätze.SelectedNode.Index, first), this.treeView_Sätze.SelectedNode.Index, first, this.detailSucheCheckBox.Checked);     //Sucht nun ueber den Index, ob es im Woerterbuch passenden Eintraege gibt.
                if (result.Rows.Count > 0)                                                  //Wenn etwas gefunden wurde, wird es in Datagridview geschrieben.
                {
                    this.dataGridView_Suchergebnisse.DataSource = result;
                }
                else                                                                        //Wenn kein Eintrag gefunden wurde, wird dataGridView_Suchergebnisse geleert und eine Meldung ausgegeben.
                {
                    dataGridView_Suchergebnisse.DataSource = null;
                    MessageBox.Show("Es wurden keine Einträge gefunden.", "Fehler bei der Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /**
         * EventFunktion: Bei klick auf den Beenden-Menüeintrag.
         * Schließt das Programm.
         */ 
        private void beendenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        /**
         * Funktion zum Löschen eines Eintrages aus der Ergebnisliste der Suche.
         * Dies ist erforderlich da nach der Änderung eines Tokens, aus der Ergebnisliste nicht der alte Eintrag wieder aufgerufen werden soll, sondern ein Neuer erstellt.
         * @param[in] satznummer int-Wert des Satzes in dem sich der zu löschende Token befindet.
         * @param[in] tok int-Wert des Tokens der gelöscht werden soll.
         */
        public void TableDel(int satznummer,int tok)
        {
            SearchEngine.DisposeTable(satznummer, tok);
        }

        /// @cond
        private static string GetDefaultBrowserPath()
        {
            string key = @"HTTP\shell\open\command";
            using (RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(key, false))
            {
                return ((string)registrykey.GetValue(null, null)).Split('"')[1];
            }
        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(GetDefaultBrowserPath(), "../../html/index.html");
        }

        /// @endcond
    }
}




