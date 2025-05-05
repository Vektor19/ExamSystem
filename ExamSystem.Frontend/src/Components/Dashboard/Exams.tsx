import { useDashboardContext } from "../../Providers/DashboardProvider";
import examsStyles from "../../Styles/Exams.module.css";
import ExaminatorsExamsBody from "./ExaminatorsExamsBody";
import StudentExamsBody from "./StudentExamsBody";

const Exams: React.FC = () => {
  const { mode } = useDashboardContext();
  return (
    <>
      <h1 className={`${examsStyles["exams-title"]}`}>Exams</h1>
      {mode === "student" ? <StudentExamsBody /> : <ExaminatorsExamsBody />}
    </>
  );
};

export default Exams;
