import { useState } from "react";
import CreateExamModal from "./CreateExamModal";
import { ExamCreate } from "../../Models/ExamCreate";
import ExamService from "../../Services/ExamService";
import { useNavigate } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";
import { NoValidRequestError } from "../../Common/Exceptions/NoValidRequestError";
import { useNotification } from "../../Providers/NotificationProvider";
const CreateExamPage: React.FC = () => {
  const [showExamModal, setShowExamModal] = useState(true);
  const { fetchExaminatorExams } = useExams();
  const navigate = useNavigate();
  const { showNotification } = useNotification();

  const handleCreateExam = async (data: ExamCreate) => {
    try {
      const examResult = await ExamService.createExam(data);
      if (!examResult) {
        showNotification("Error creating exam", "error");
        navigate("/dashboard/exam-management");
        return;
      }
      await fetchExaminatorExams();
      showNotification("Exam created successfully", "success");
      navigate("/dashboard/edit-exam/" + examResult.examId);
    } catch (err: any) {
      if (err instanceof NoValidRequestError) {
        const errors = JSON.parse(err.message);
        console.log("Validation errors:", errors);
        showNotification(
          Object.entries(errors).map(
            ([_, messages]) => `${(messages as string[]).join(", ")}`
          )[0],
          "error"
        );
      } else {
        showNotification("Error creating exam", "error");
        console.error(err);
        navigate("/dashboard/exam-management");
      }
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
