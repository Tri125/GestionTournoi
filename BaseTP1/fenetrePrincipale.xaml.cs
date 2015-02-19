using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaseTP1
{
    /// <summary>
    /// Logique d'interaction pour fenetrePrincipale.xaml
    /// </summary>
    public partial class fenetrePrincipale : Window
    {
        private ObservableCollection<Joueur> listeParticipantsTournoi;
        private ObservableCollection<Joueur> listeJoueurs;
        private bool isModified = false;

        public fenetrePrincipale()
        {
            InitializeComponent();
            listeJoueurs = new ObservableCollection<Joueur>();
            listeParticipantsTournoi = new ObservableCollection<Joueur>();
            dgTournois.ItemsSource = listeParticipantsTournoi;
            dgJoueur.ItemsSource = listeJoueurs;
            dgTournois.CanUserAddRows = true;
            btnFlecheDroite.Content = "\u2192";
            btnFlecheGauche.Content = "\u2190";
        }

        private void btnFlecheGauche_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList t = dgTournois.SelectedItems;
            for (int i = t.Count; i != 0; i--)
            {
                listeParticipantsTournoi.Remove((Joueur)t[0] );
            }
        }

        private void btnFlecheDroite_Click(object sender, RoutedEventArgs e)
        {
            foreach (Joueur j in dgJoueur.SelectedItems)
            {
                if (!listeParticipantsTournoi.Contains(j))
                    listeParticipantsTournoi.Insert(0, j);
            }
        }


        private void btnChargement_Click(object sender, RoutedEventArgs e)
        {
            isModified = false;
            listeJoueurs.Clear();
            listeParticipantsTournoi.Clear();
            StringBuilder chaineListe = new StringBuilder();

            // Test de la lecture de fichier, un élément.
            foreach (Joueur j in Joueur.chargerListeJoueurs())
            {
                listeJoueurs.Add(j);
            }
            foreach (Joueur j in listeJoueurs)
            {
                chaineListe.Append("#").Append(j.NoDCI).Append(" : ").Append(j.Prenom).Append(" ").Append(j.Nom).AppendLine(".");
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Joueur.enregistrerListeJoueurs(listeJoueurs.ToList());
            isModified = false;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isModified)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void dgJoueur_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            isModified = true;
        }

        private void Appariment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            Stack<Joueur> stackPairing;
            List<Joueur> listePairing = new List<Joueur>(listeParticipantsTournoi);
            Joueur.randomListeJoueurs(listePairing);
            stackPairing = new Stack<Joueur>(listePairing);
            bool isEven = (listePairing.Count % 2) == 0;
            bool isNewPair = false;
            while (stackPairing.Count != 0)
            {
                message.Append(stackPairing.Pop().Nom);
                if (stackPairing.Count != 0 && !isNewPair)
                    message.Append(" vs ");
                else
                    if (isNewPair)
                        message.Append("\n\n");
                isNewPair = !isNewPair;

            }
            if (!isEven)
            {
                message.Append(" : Bye\n");
            }
            MessageBox.Show(message.ToString(),"Appariement tour #1",MessageBoxButton.OK,MessageBoxImage.Information);


        }

        private void Appariment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (listeParticipantsTournoi.Count >= 2)
            {
                e.CanExecute = true;
            }
            else
                e.CanExecute = false;
        }


    }
}
