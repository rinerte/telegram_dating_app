import botImg from "../assets/profile_1.png";

const BotImage = () => {
    const tData = window.Telegram.WebApp.initDataUnsafe;
    let userName = null;
    let userId = null;

    try{
        userName = tData.user.first_name;
        userId = tData.user.id;
        
    } catch {
        userName = 'UserName';
        
    }

    return (
        <div className="
    inline-flex
    p-0 m-0
     ">
        <img src={botImg} alt="BotImage" className="
        h-[150px] w-[145px] -translate-x-[20px] translate-y-[50px] inline
        " />
        <div className="
        flex justify-center items-center
        px-2 py-2
        mt-[10px]  min-w-[50px] -translate-x-[20px] h-fit
        font-prostoOne text-[25px] font-light 
        bg-white
        border border-primaryBlack rounded-[20px] rounded-bl-none
        ">
            Привет, <br/> {userName} !
        </div>

    </div>
    )
}

export default BotImage;