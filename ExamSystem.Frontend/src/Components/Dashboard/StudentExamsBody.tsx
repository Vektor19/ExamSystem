import { StudentExam } from "../../Models/StudentExam";
import { useExams } from "../../Providers/ExamsProvider";
import DashboardPaper from "../Papers/DashboardPaper";

const StudentExamsBody: React.FC = () => {
  const { examinatorExams } = useExams();

  return (
    <>
      <DashboardPaper className="students-exams">
        <h1 className="student-exams-title">Student Exams</h1>
        <div className="student-exams-list">
          {examinatorExams?.map(
            (exam: StudentExam | null) =>
              exam && (
                <div key={exam.examId} className="student-exam-item">
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

export default StudentExamsBody;
