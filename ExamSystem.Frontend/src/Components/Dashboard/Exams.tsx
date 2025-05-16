import { useDashboardContext } from "../../Providers/DashboardProvider";
import examsStyles from "../../Styles/Exams.module.css";
import StudentExamsBody from "./StudentExamsBody";
import ExaminatorExamsBody from "./ExaminatorExamsBody";

const Exams: React.FC = () => {
  const { mode } = useDashboardContext();
  return (
    <>
      <h1 className={`${examsStyles["exams-title"]}`}>Exams</h1>
      {mode === "student" ? <StudentExamsBody /> : <ExaminatorExamsBody />}
    </>
  );
};

export default Exams;
