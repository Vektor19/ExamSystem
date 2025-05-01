import { Link } from "react-router-dom";
import homeStyles from "../../Styles/Home.module.css";
import outletStyles from "../../Styles/PublicOutlet.module.css";
import PrimaryButton from "../Buttons/PrimaryButton";
import mainLogo from "/home_page.png";

const Home = () => {
  return (
    <>
      <section className={`${outletStyles["public-outlet"]}`}>
        <div className={`${homeStyles["home-page-container"]} `}>
          <div className={homeStyles["home-page-introduction"]}>
            <div className={homeStyles["home-page-introduction-text"]}>
              <h1>Welcome to the Exam System</h1>
              <h3>This is the home page of our application.</h3>
              <Link to="/dashboard">
                <PrimaryButton
                  size="medium"
                  variant="contained"
                  className={homeStyles["home-page-introduction-button"]}
                >
                  Get Started
                </PrimaryButton>
              </Link>
            </div>

            <img
              src={mainLogo}
              alt="Main Page"
              className={homeStyles["main-page-logo"]}
            />
          </div>
        </div>
      </section>
    </>
  );
};

export default Home;
