import BotImage from "./BotImage";
import Label from "./Label";

function Header({IsImage}) {
  return (
    <div className="absolute w-full md:w-[600px] top-[10px]">
        {IsImage? <BotImage/> : <Label/>}
    </div>
    
  )
}

export default Header