import styles from "../../Styles/HomeHeader.module.css";
import HomeNavigation from "./HomeNavigation";
import viteLogo from "/vite.svg";
const HomeHeader = () => {
  return (
    <>
      <header className={styles["home-header"]}>
        <div className={styles["home-header-info"]}>
          <a href="/">
            <img src={viteLogo} className={styles["logo"]} alt="Vite logo" />
            <h3>Exam System</h3>
          </a>
        </div>
        <HomeNavigation />
      </header>
    </>
  );
};

export default HomeHeader;
