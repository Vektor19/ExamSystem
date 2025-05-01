import { Link } from "react-router-dom";
import styles from "../../Styles/DashboardNavigation.module.css";
import viteLogo from "/vite.svg";

import { Switch } from "@mui/material";
import { useDashboardContext } from "../../Providers/DashboardProvider";

const DashboardNavigation = () => {
  const { mode, setMode } = useDashboardContext();

  const handleModeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setMode(event.target.checked ? "examinator" : "student");
  };

  const examsLink = mode === "student" ? "/my-exams" : "/exam-management";

  return (
    <div className={`${styles["dashboard-navigation-container"]}`}>
      <div className={`${styles["dashboard-navigation-header"]}`}>
        <Link to="/">
          <img src={viteLogo} className={styles["logo"]} alt="Vite logo" />
        </Link>
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
            <Link
              to="/dashboard"
              className={`${styles["dashboard-navigation-link"]}`}
            >
              Dashboard
            </Link>
          </li>
          <li className={`${styles["dashboard-navigation-item"]}`}>
            <Link
              to="/dashboard/profile"
              className={`${styles["dashboard-navigation-link"]}`}
            >
              Profile
            </Link>
          </li>
          <li className={`${styles["dashboard-navigation-item"]}`}>
            <Link
              to={examsLink}
              className={`${styles["dashboard-navigation-link"]}`}
            >
              Exams
            </Link>
          </li>
        </ul>
      </nav>

      <div>
        <Link to="/logout" className={`${styles["dashboard-logout"]}`}>
          Logout
        </Link>
      </div>
    </div>
  );
};

export default DashboardNavigation;
