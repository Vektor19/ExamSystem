import { Paper } from "@mui/material";
import studentDashboardStyles from "../../Styles/StudentDashboardBody.module.css";
const StudentDashboardBody = () => {
  return (
    <>
      <div className={`${studentDashboardStyles["student-dashboard-body"]}`}>
        <Paper>
          <h1
            className={`${studentDashboardStyles["student-dashboard-title"]}`}
          >
            Student Dashboard
          </h1>
          <p
            className={`${studentDashboardStyles["student-dashboard-subtitle"]}`}
          >
            View your exams, results, and more.
          </p>
        </Paper>
        <Paper>
          <h1
            className={`${studentDashboardStyles["student-dashboard-title"]}`}
          >
            Student Dashboard
          </h1>
          <p
            className={`${studentDashboardStyles["student-dashboard-subtitle"]}`}
          >
            View your exams, results, and more.
          </p>
        </Paper>
        <Paper>
          <h1
            className={`${studentDashboardStyles["student-dashboard-title"]}`}
          >
            Student Dashboard
          </h1>
          <p
            className={`${studentDashboardStyles["student-dashboard-subtitle"]}`}
          >
            View your exams, results, and more.
          </p>
        </Paper>
        <Paper>
          <h1
            className={`${studentDashboardStyles["student-dashboard-title"]}`}
          >
            Student Dashboard
          </h1>
          <p
            className={`${studentDashboardStyles["student-dashboard-subtitle"]}`}
          >
            View your exams, results, and more.
          </p>
        </Paper>
      </div>
    </>
  );
};

export default StudentDashboardBody;
