import homeStyles from "../../Styles/Home.module.css";
import outletStyles from "../../Styles/PublicOutlet.module.css";
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
              <Button
                variant="contained"
                color="primary"
                className={homeStyles["home-page-button"]}
                onClick={() => {
                  window.location.href = "/dashboard";
                }}
                sx={{
                  fontSize: '1.1em',
                  fontWeight: '700',
                  '&:hover': {
                    backgroundColor: 'primary.dark',
                    color: 'primary.light',
                  }
                }}
              >
                Get Started
              </Button>
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
