import styles from "../../Styles/DashboardNavigation.module.css";
import viteLogo from "/vite.svg";
const DashboardNavigation = () => {
  return (
    <div className={`${styles["dashboard-navigation-container"]}`}>
      <div className={`${styles["dashboard-navigation-header"]}`}>
        <a href="/">
          <img src={viteLogo} className={styles["logo"]} alt="Vite logo" />
        </a>
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
              href="/dashboard/exams"
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
