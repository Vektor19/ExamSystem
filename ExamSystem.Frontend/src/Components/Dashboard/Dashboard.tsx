import { useEffect, useState } from "react";
import { Snackbar, Alert } from "@mui/material";
import dashboardStyles from "../../Styles/Dashboard.module.css";
import { useDashboardContext } from "../../Providers/DashboardProvider";
import StudentDashboardBody from "./StudentDashboardBody";
import ExaminatorDashboardBody from "./ExaminatorDashboardBody";
const Dashboard = () => {
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");
  const { mode } = useDashboardContext();

  useEffect(() => {
    setSnackbarMessage("Welcome to the Dashboard!");
    setOpenSnackbar(true);
    setTimeout(() => {
      setOpenSnackbar(false);
    }, 3000);
  }, []);
  return (
    <>
      <h1 className={`${dashboardStyles["dashboard-title"]}`}>Overview</h1>
      {mode === "student" ? <StudentDashboardBody /> : <ExaminatorDashboardBody/>}
      <Snackbar
        open={openSnackbar}
        onClose={() => setOpenSnackbar(false)}
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
      >
        <Alert onClose={() => setOpenSnackbar(false)} severity="success">
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </>
  );
};

export default Dashboard;
