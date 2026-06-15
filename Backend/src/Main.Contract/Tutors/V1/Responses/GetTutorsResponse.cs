using Main.Contract.Tutors.V1.DTO;

namespace Main.Contract.Tutors.V1.Responses;

public class GetTutorsResponse
{
    public List<TutorMetaDTO> TutorsMeta { get; set; } = [];
}