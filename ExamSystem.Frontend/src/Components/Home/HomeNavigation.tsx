import styles from "../../Styles/HomeNavigation.module.css";

const HomeNavigation = () => {
  return (
    <>
      <nav className={styles["home-navigation"]}>
        <ul>
          <li>
            <a href="/">Home</a>
          </li>
          <li>
            <a href="/">About</a>
          </li>
          <li>
            <a href="/">Contact</a>
          </li>
          <li>
            <a href="/">Get started</a>
          </li>
        </ul>
      </nav>
    </>
  );
};

export default HomeNavigation;
