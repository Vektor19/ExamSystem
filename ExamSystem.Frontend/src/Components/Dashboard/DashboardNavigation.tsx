import { useState } from "react";
import styles from "../../Styles/DashboardNavigation.module.css";
import viteLogo from "/vite.svg";

import { Switch, FormControlLabel, Typography } from "@mui/material";
import { useDashboardContext } from "../../Providers/DashboardProvider";

const DashboardNavigation = () => {
  const {mode, setMode} = useDashboardContext();

  const handleModeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setMode(event.target.checked ? "examinator" : "student");
  };

  const examsLink = mode === "student" ? "/my-exams" : "/exam-management";

  return (
    <div className={`${styles["dashboard-navigation-container"]}`}>
      <div className={`${styles["dashboard-navigation-header"]}`}>
        <a href="/">
          <img src={viteLogo} className={styles["logo"]} alt="Vite logo" />
        </a>
        <div className={`${styles["dashboard-navigation-mode"]}`}>
          <h4 style={{ fontWeight: mode === "examinator" ? "bold" : "normal" }}>
            Examinator
          </h4>
          <Switch
            checked={mode === "examinator"}
            onChange={handleModeChange}
            color="secondary"
            sx={{
              transform: "rotate(-90deg)",
              transformOrigin: "center",
              alignSelf: "center",
              "& .MuiSwitch-track": {
                backgroundColor: "secondary.main",
              },
            }}
          />
          <h4 style={{ fontWeight: mode === "student" ? "bold" : "normal" }}>
            Student
          </h4>
        </div>
      </div>

      <nav className={`${styles["dashboard-navigation"]}`}>
        <ul className={`${styles["dashboard-navigation-list"]}`}>
          <li className={`${styles["dashboard-navigation-item"]}`}>
            <a
              href="/dashboard"
              className={`${styles["dashboard-navigation-link"]}`}
            >
              Dashboard
            </a>
          </li>
          <li className={`${styles["dashboard-navigation-item"]}`}>
            <a
              href="/dashboard/profile"
              className={`${styles["dashboard-navigation-link"]}`}
            >
              Profile
            </a>
          </li>
          <li className={`${styles["dashboard-navigation-item"]}`}>
            <a
              href={examsLink}
              className={`${styles["dashboard-navigation-link"]}`}
            >
              Exams
            </a>
          </li>
        </ul>
      </nav>

      <div>
        <a href="/logout" className={`${styles["dashboard-logout"]}`}>
          Logout
        </a>
      </div>
    </div>
  );
};

export default DashboardNavigation;
