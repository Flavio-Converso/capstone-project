using System.ComponentModel.DataAnnotations;

namespace capstone_project.Models.DTOs.Game
{
    public class GameDTO
    {
        public int GameId { get; set; }

        [Required(ErrorMessage = "Il nome del gioco è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "Il nome del gioco non può superare i 100 caratteri.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descrizione del gioco è obbligatoria.")]
        [MaxLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La piattaforma del gioco è obbligatoria.")]
        [MaxLength(50, ErrorMessage = "Il nome della piattaforma non può superare i 50 caratteri.")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "Il produttore del gioco è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "Il nome del produttore non può superare i 100 caratteri.")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Il prezzo del gioco è obbligatorio.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un valore positivo.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La data di rilascio è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di rilascio non è valida.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "La quantità disponibile è obbligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantità disponibile deve essere un numero positivo.")]
        public int QuantityAvail { get; set; }

        [MaxLength(500, ErrorMessage = "Il link del video di YouTube non può superare i 500 caratteri.")]
        public string? VideoPath { get; set; }
        [Required(ErrorMessage = "La classificazione PEGI è obbligatoria.")]
        public int PegiId { get; set; }

        [Required(ErrorMessage = "È necessario selezionare almeno una restrizione.")]
        public List<int> RestrictionIds { get; set; } = [];

        [Required(ErrorMessage = "È necessario selezionare almeno una categoria.")]
        public List<int> CategoryIds { get; set; } = [];
        public List<GameImageDTO> GameImages { get; set; } = [];

    }
}
