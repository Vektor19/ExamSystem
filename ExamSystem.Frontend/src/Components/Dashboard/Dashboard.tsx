import React, { useEffect, useState } from "react";
import { Snackbar, Alert } from "@mui/material";
import outletStyles from "../../Styles/DashboardOutlet.module.css";
import dashboardStyles from "../../Styles/Dashboard.module.css";
const Dashboard = () => {
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");

  useEffect(() => {
    setSnackbarMessage("Welcome to the Dashboard!");
    setOpenSnackbar(true);
    setTimeout(() => {
      setOpenSnackbar(false);
    }, 3000);
  }, []);
  return (
    <>
      <section className={`${outletStyles["dashboard-outlet"]}`}>
        <div className={`${dashboardStyles["dashboard-page-container"]} `}>
          <h1>Welcome to the Exam System</h1>
          <h3>This is the dashboard page of our application.</h3>
          <p>
            You can navigate to different sections of the application using the
            navigation bar.
          </p>
        </div>
      </section>
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
