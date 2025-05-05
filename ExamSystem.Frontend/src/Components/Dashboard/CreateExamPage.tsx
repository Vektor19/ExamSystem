import { useState } from "react";
import CreateExamModal from "./CreateExamModal";
import { ExamCreate } from "../../Models/ExamCreate";
import ExamService from "../../Services/ExamService";
import { useNavigate } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";

const CreateExamPage: React.FC = () => {
  const [showExamModal, setShowExamModal] = useState(true);
  const { fetchExaminatorExams } = useExams();
  const navigate = useNavigate();

  const handleCreateExam = async (data: ExamCreate) => {
    try {
      const examResult = await ExamService.createExam(data);
      if (!examResult) {
        navigate("/dashboard/exam-management");
        return;
      }
      await fetchExaminatorExams();
      navigate("/dashboard/edit-exam/" + examResult.examId);
    } catch (err) {
      console.error(err);
      navigate("/dashboard/exam-management");
    }
  };
  return (
    <>
      <CreateExamModal
        open={showExamModal}
        onClose={() => {
          setShowExamModal(false);
          navigate("/dashboard/exam-management");
        }}
        onCreate={handleCreateExam}
      />
    </>
  );
};

export default CreateExamPage;
