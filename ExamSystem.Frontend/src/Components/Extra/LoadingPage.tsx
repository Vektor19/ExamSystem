const LoadingPage = () => {
  return (
    <div
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        width: "100vw",
        height: "100vh",
        backgroundColor: "var(--primary-light-color)",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        zIndex: 9999,
      }}
    >
      <div style={{ textAlign: "center" }}>
        <img
          src="https://i.pinimg.com/originals/71/94/64/719464cf88c8e2ef95107b96f5adf2d3.gif"
          alt="Loading..."
          style={{ width: "30vh", height: "30vh" }}
        />
        <h2 style={{ fontFamily: '"Lato", sans-serif', color: "var(--primary-main-color)", fontSize: "2rem" }}>
          Loading, please wait...
        </h2>
      </div>
    </div>
  );
};

export default LoadingPage;
