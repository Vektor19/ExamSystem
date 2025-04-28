import homeStyles from "../../Styles/Home.module.css";
import outletStyles from "../../Styles/PublicOutlet.module.css";

const Home = () => {
  return (
    <>
      <section className={`${outletStyles["public-outlet"]}`}>
        <div className={`${homeStyles["home-page-container"]} `}>
          <h1>Welcome to the Exam System</h1>
          <p>This is the home page of our application.</p>
        </div>
      </section>
    </>
  );
};

export default Home;
