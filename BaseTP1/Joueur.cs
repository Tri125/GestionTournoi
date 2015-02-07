using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BaseTP1
{
    /// <summary>
    /// Un joueur.
    /// </summary>
    public class Joueur
    {
        #region Static

        /// <summary>
        /// Lit la liste des joueurs (ceux qui se retrouvent dans le fichier .csv lié à l'application) et retourne une liste.
        /// La gestion d'erreurs faite est minimale.  Si le fichier est incorrect ou corrompu, la méthode retournera une liste vide.
        /// </summary>
        /// <returns>La liste des joueurs.</returns>
        public static List<Joueur> chargerListeJoueurs()
        {
            StreamReader fichierJoueurs;
            List<Joueur> lstJoueurs = new List<Joueur>();
            string[] tDonneesLigne;

            // Gestion simple des erreurs.  S'il y en a une, on retourne une liste vide.
            try
            {
                fichierJoueurs = new StreamReader(File.OpenRead("ListeJoueurs.csv"));

                while (!fichierJoueurs.EndOfStream)
                {
                    // Lire une ligne et la séparer en ses éléments distincts.
                    tDonneesLigne = (fichierJoueurs.ReadLine()).Split(';');

                    // Créer un nouvel objet Joueur avec les données de la ligne et l'ajouter à la liste.
                    lstJoueurs.Add(new Joueur( tDonneesLigne[0]
                                             , tDonneesLigne[1]
                                             , tDonneesLigne[2]
                                             )
                                   );
                }

                fichierJoueurs.Close();
            }
            catch {}

            return lstJoueurs;
        }

        /// <summary>
        /// Écrit (dans le fichier .csv lié à l'application) les réservations qui se trouve dans la liste recue.
        /// La gestion d'erreurs faite est minimale.  Si une erreur se produit, le méthode se termine et ne fait rien de plus.
        /// </summary>
        /// <param name="lstReservations">La liste des réservations qui doivent être enregistrées dans le fichier.</param>
        public static void enregistrerListeJoueurs(List<Joueur> lstJoueurs)
        {
            // TODO: FileSTream(path, Truncate, Write, None) pour vider le fichier ... problématique, vide le fichier!!!
            // Faire un mode ajout!

            StreamWriter fichierJoueurs;
            StringBuilder strLigneJoueur = new StringBuilder();

            try
            {
                //fichierReservations = new StreamWriter(File.OpenWrite("ListeReservations.csv"));
                fichierJoueurs = new StreamWriter(new FileStream("ListeJoueurs.csv", FileMode.Truncate, FileAccess.Write, FileShare.None));

                foreach (Joueur j in lstJoueurs)
                {
                    // Batir la ligne à entrer dans le fichier.
                    strLigneJoueur.Clear();
                    strLigneJoueur.Append(j.NoDCI).Append(";").Append(j.Prenom).Append(";").Append(j.Nom);

                    // Écrire l'entrée dans le fichier.
                    fichierJoueurs.WriteLine(strLigneJoueur.ToString());
                }

                fichierJoueurs.Close();
            }
            catch { }
        }

        /// <summary>
        /// Repositionne les joueurs de la liste au hasard à l'intérieur de la liste.
        /// </summary>
        /// <param name="lstJoueurs">La liste qui doit être travaillée par l'algorithme</param>
        public static void randomListeJoueurs(List<Joueur> lstJoueurs)
        { 
            // C'est à vous de coder cette partie. 
            // Utilisez l'algorithme Fisher-Yates (facile à trouver sur internet) et respectez le "contrat" de la méthode.
        }

        #endregion

        public string NoDCI { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }

        /// <summary>
        /// Constructeur de la classe Joueur.
        /// </summary>
        /// <param name="noDCI">Le numéro unique attribué au joueur par l'organisme DCI.</param>
        /// <param name="prenom">Le prenom du joueur.</param>
        /// <param name="nom">Le nom de famille du joueur.</param>
        public Joueur(string noDCI, string prenom, string nom)
        {
            NoDCI = noDCI;
            Prenom = prenom;
            Nom = nom;
        }
    }
}
