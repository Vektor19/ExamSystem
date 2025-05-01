import { Paper } from "@mui/material";
import studentDashboardStyles from "../../Styles/StudentDashboardBody.module.css";
import DashboardPaper from "../Papers/DashboardPaper";
const StudentDashboardBody = () => {
  return (
    <>
      <div className={`${studentDashboardStyles["student-dashboard-body"]}`}>
        <DashboardPaper className={`${studentDashboardStyles["upcoming-exams-container"]}`}>
          <h3>Upcoming Exams</h3>
          <ul>
            <li>
              Physics — <em>May 15, 2025 at 10:00</em>
            </li>
            <li>
              English — <em>May 20, 2025 at 12:00</em>
            </li>
          </ul>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Recent Results</h3>
          <ul>
            <li>Mathematics — 88%</li>
            <li>Programming — 94%</li>
          </ul>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Notifications</h3>
          <ul>
            <li>🔔 New message from your examiner</li>
            <li>✅ Programming exam graded</li>
          </ul>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Resources</h3>
          <ul>
            <li>📚 Study materials for upcoming exams</li>
            <li>📝 Practice tests and solutions</li>
          </ul>
        </DashboardPaper>
      </div>
    </>
  );
};

export default StudentDashboardBody;
