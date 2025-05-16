import { Link } from "react-router-dom";
import styles from "../../Styles/HomeNavigation.module.css";

const HomeNavigation = () => {
  return (
    <nav className={styles["home-navigation"]}>
      <ul>
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          <Link to="/about">About</Link>
        </li>
        <li>
          <Link to="/contact">Contact</Link>
        </li>
        <li>
          <Link to="/get-started">Get started</Link>
        </li>
      </ul>
    </nav>
  );
};

export default HomeNavigation;
