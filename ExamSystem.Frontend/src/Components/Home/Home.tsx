import homeStyles from "../../Styles/Home.module.css";
import outletStyles from "../../Styles/PublicOutlet.module.css";
import PrimaryButton from "../Buttons/PrimaryButton";
import mainLogo from "/home_page.png";
import { Button } from "@mui/material";

const Home = () => {
  return (
    <>
      <section className={`${outletStyles["public-outlet"]}`}>
        <div className={`${homeStyles["home-page-container"]} `}>
          <div className={homeStyles["home-page-introduction"]}>
            <div className={homeStyles["home-page-introduction-text"]}>
              <h1>Welcome to the Exam System</h1>
              <h3>This is the home page of our application.</h3>
              <PrimaryButton
                size="medium"
                variant="contained"
                className={homeStyles["home-page-introduction-button"]}
                onClick={() => {
                  window.location.href = "/dashboard";
                }}
              >
                Get Started
              </PrimaryButton>
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
