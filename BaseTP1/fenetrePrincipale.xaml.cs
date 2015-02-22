using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        //ObservableCollection pour avoir accès à plusieurs évènements lancé suite à la modification de données. Permet de faire du dataBinding en deux-sens.
        private ObservableCollection<Joueur> listeParticipantsTournoi;
        private ObservableCollection<Joueur> listeJoueurs;
        //Utilisé pour signaler la modification de la collection listeJoueurs et activer le boutton pour écraser la liste de joueurs.
        private bool isModified = false;

        public fenetrePrincipale()
        {
            InitializeComponent();
            listeJoueurs = new ObservableCollection<Joueur>();
            listeParticipantsTournoi = new ObservableCollection<Joueur>();
            //Enregistrement de la méthode OnCollectionChanged à l'évènement CollectionChanged. La méthode sera exécuté au lancement de l'évènement.
            listeJoueurs.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            //Spécifie l'objet source du dataGrid dgTournoi. Les données affiché seront celles contenues dans l'ObservableCollection.
            dgTournoi.ItemsSource = listeParticipantsTournoi;
            dgJoueur.ItemsSource = listeJoueurs;
            //Empêche l'utilisateur de rajouter une ligne dans la dataGrid des inscription du tournoi.
            //Chaque joueur devrait être déjà dans la liste et rajouté en appuyant sur un bouton. Comportement déjà enforcé puisqu'elle est en lecture seule.
            dgTournoi.CanUserAddRows = false;
            //Code UTF-8 pour les caractères de flèches (droite/gauche).
            btnFlecheDroite.Content = "\u2192";
            btnFlecheGauche.Content = "\u2190";
        }


        /// <summary>
        /// Méthode pour détecter lorsqu'une suppression d'un élément d'un ObservableCollection à lieu
        /// Signale que le bouton de sauvegarde devrait être activé et les retire automatiquement de la liste des joueurs inscrits au tournoi.
        /// </summary>
        /// <param name="sender">Objet source</param>
        /// <param name="e">Argument de l'évènement CollectionChanged</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Si l'action est une action de suppression.
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                /* Par conception, les seuls objets qui devrait ce trouver dans les ObservableCollections employées sont de type Joueur.
                 * Il est donc inutile de faire des testes de conversion.
                 * De la liste des objets nouvellement supprimés, retire les également de listeParticipantsTournoi.
                 * Si des joueur sont supprimés de notre base de connaissance, ils se veront désinscrits du tournoi automatiquement.
                 */
                foreach (Joueur j in e.OldItems)
                {
                    /* Idéalement, une fenêtre d'avertissement qui demande la confirmation à l'usager serait présentée pour empêcher les erreurs/accidents, mais l'évènement
                     * CollectionChanged est lancé après la modification, les éléments sont donc déjà supprimés. Il est impossible de rajouter les éléments pendant
                     * que l'évènement est en exécution, la collection ne peut être modifier. Il est possible de le faire en interceptant la touche de suppression avant
                     * que la collection soit modifié, ou en utilisant de la programmation utilisant un thread différent et sans doute en mode unsafe.
                     * En résumé, sa ne veut pas la peine pour notre application actuel.
                     */
                    listeParticipantsTournoi.Remove(j);
                }
                //Drapeau signalant que la collection est modifiée.
                isModified = true;
            }
        }

        /// <summary>
        /// Méthode lancé à l'exécution du click event de btnFlecheGauche. Retire la sélection du dataGrid dgTournoi de listeParticipantsTournoi. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFlecheGauche_Click(object sender, RoutedEventArgs e)
        {
            //Si plusieurs lignes sont sélectionnés, nous voulons tous les retirer. SelectedItems retourne un objet System.Collections.IList les contenants.
            System.Collections.IList t = dgTournoi.SelectedItems;
            /* Il faut penser que lors du retrait, SelectedItems est modifiée et les index des éléments restant sont réduit de 1.
             * ex: A(0), B(1), C(2). Au retrait de A, SelectedItems devient immédiatement: B(0), C(1).
             * Nous sommes assuré que si des éléments sont sélectionnées, t[0] existe. Donc, pas besoin de faire des testes pour ne pas dépasser le range, sort lorsque i == 0.
             */
            for (int i = t.Count; i != 0; i--)
            {
                //Par conception, SelectedItems sont des Joueur et nous pouvons forcer le cast sans test.
                listeParticipantsTournoi.Remove((Joueur)t[0]);
            }
        }

        /// <summary>
        /// Méthode lancé à l'exécution du click event de btnFlecheDroite. Rajoute la sélection du dataGrid dgJoueur dans listeParticipantsTournoi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFlecheDroite_Click(object sender, RoutedEventArgs e)
        {
            //La seule fois où il est pratiquement nécessaire d'utiliser un var.
            //Si l'utilisateur sélectionne la ligne vide de création d'une nouvelle entrée, il sera d'un autre type que Joueur.
            //Il ne faut donc pas faire un foreach des Joueur de la liste, car une exception sera lancé dans ce cas d'utilisation.
            foreach (var item in dgJoueur.SelectedItems)
            {
                //Teste de conversion à l'exécution, puis comparaison à null.
                Joueur joueur = item as Joueur;
                if (joueur != null)
                {
                    //Si listeParticipantsTournoi ne contient déjà pas joueur, le rajouter.
                    if (!listeParticipantsTournoi.Contains(joueur))
                        listeParticipantsTournoi.Insert(0, joueur);
                }
            }
        }

        /// <summary>
        /// Méthode lancé lors de l'exécution du click event de btnChargement. Bouton pour ouvrir le fichier de données et charger dans listeJoueurs les joueurs
        /// qui si trouvent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChargement_Click(object sender, RoutedEventArgs e)
        {
            //Remet à false le drapeau pour désactivé le bouton de sauvegarde.
            isModified = false;
            //Vide la liste, l'application ne prend pas en charge le chargement multiple de fichiers.
            listeJoueurs.Clear();
            //Vide la liste.
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

        /// <summary>
        /// Méthode de la commande Save de btnSauvegarde. Lancé lorsque la commande est exécuté, en autre lorsque le bouton est cliqué.
        /// Enregistre la liste de joueurs de listeJoueurs dans le fichier de données.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Remet à faux le drapeau qui signale que la liste a été modifié et lance la méthode statique enregistrerListeJoueurs de la classe Joueur.
            //Conversion nécessaire pour respecter les paramètres.
            Joueur.enregistrerListeJoueurs(listeJoueurs.ToList());
            isModified = false;
        }

        /// <summary>
        /// Méthode de la commande Save de btnSauvegarde. Lancé continuellement pour vérifier si la commande Save est en mesure de s'exécuter ou non.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Si le drapeau de modification est vrai, l'argument de l'évènement est utilisé pour activé la commande. (Le bouton de sauvegarde sera activé)
            if (isModified)
                e.CanExecute = true;
            else
                //Sinon la commande est désactivé tout comme le bouton.
                e.CanExecute = false;
        }

        /// <summary>
        /// Méthode de la commande NotACommand de btnAppariment. Lancé lorsque le bouton est cliqué.
        /// Génère l'appariment entre joueur du tournoi et l'affiche dans un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appariment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            //L'utilisation du pile réduit les testes nécessaires, suffit de retirer le joueur de la pile une fois qu'il a son adversaire.
            Stack<Joueur> stackPairing;
            //Création d'une List<Joueur> pour la méthode statique randomListeJoueurs de la classe Joueur.
            List<Joueur> listePairing = new List<Joueur>(listeParticipantsTournoi);
            Joueur.randomListeJoueurs(listePairing);
            //Initialise la pile avec les éléments de listePairing.
            stackPairing = new Stack<Joueur>(listePairing);
            //Vérification si la liste est de nombre paire, si non le dernier joueur à une mention bye pour son appariment.
            bool isEven = (listePairing.Count % 2) == 0;
            //Une nouvelle paire d'adversaire. À vrai c'est le premier joueur de son appariment, à faux c'est le deuxième joueur.
            bool isNewPair = true;
            while (stackPairing.Count != 0)
            {
                message.Append(stackPairing.Pop().Nom);
                //S'il reste d'autres joueurs et que c'est le premier joueur de la paire.
                if (stackPairing.Count != 0 && isNewPair)
                    message.Append(" contre ");
                else
                    //Si c'est le deuxième joueur de la paire.
                    if (!isNewPair)
                        message.Append("\n\n");
                isNewPair = !isNewPair;

            }
            //Si l'appariment ne contient pas un nombre paire de joueur, le dernier à donc une mention de bye.
            if (!isEven)
            {
                message.Append(" : Bye\n");
            }
            MessageBox.Show(message.ToString(), "Appariement ronde #1", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        /// <summary>
        /// Méthode de la commande NotACommand de btnAppariment. Lancé continuellement pour vérifier que la commande peut s'exécuter ou non.
        /// Vérifie si les conditions pour lancer un appariment de tournoi est respecté. Active le bouton si oui, le désactive dans le cas contraire.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appariment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Il faut au moins deux joueurs pour un tournoi, non?
            if (listeParticipantsTournoi.Count >= 2)
            {
                e.CanExecute = true;
            }
            else
                e.CanExecute = false;
        }

        /// <summary>
        /// Méthode exécuté lors du lancement de l'évènement InitializingNewItem du dataGrid dgJoueur.
        /// Utilisé pour initialiser les champs avec des valeures par défault pour un nouveau objet du dataGrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgJoueur_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            //Par conception, le dataGrid contient uniquement des objets de type Joueur, il est donc possible de forcer le cast sans vérification.
            Joueur nvJoueur = (Joueur)e.NewItem;
            nvJoueur.Nom = "NomJoueur";
            nvJoueur.Prenom = "PrénomJoueur";
            nvJoueur.NoDCI = "#DCI";
        }

        /// <summary>
        /// Méthode exécuté lors du lancement de l'évènement CellEditEnding du dataGrid dgJoueur.
        /// Utilisé pour détecter lorsqu'un champs est possiblement modifié et activé la commande Save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgJoueur_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Le drapeau est mit à true et la commande Save sera activée.
            isModified = true;
        }

    }
}
