import { Exam } from "../../Models/Exam";
import { useExams } from "../../Providers/ExamsProvider";
import DashboardPaper from "../Papers/DashboardPaper";

const ExaminatorExamsBody: React.FC = () => {
  const { examinatorExams } = useExams();

  return (
    <>
      <DashboardPaper className="examinator-exams-paper">
        <h1 className="examinator-exams-title">Examinator Exams</h1>
        <div className="examinator-exams-list">
          {examinatorExams?.map(
            (exam: Exam | null) =>
              exam && (
                <div key={exam.examId} className="examinator-exam-item">
                  <h2>{exam.name}</h2>
                  <p>Status: {exam.status}</p>
                  <p>Start Date: {exam.startDate}</p>
                  <p>End Date: {exam.endDate}</p>
                  <p>Created By: {exam.createdBy.lastName}</p>
                  <p>Questions: {exam.questionCount}</p>
                  <p>Participants: {exam.participantCount}</p>
                </div>
              )
          )}
        </div>
      </DashboardPaper>
    </>
  );
};

export default ExaminatorExamsBody;
