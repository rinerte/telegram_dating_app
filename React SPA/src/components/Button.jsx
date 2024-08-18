const Button = ({Caption, Selected, Style, OnClick, Tip, Disabled, Animate}) => {

    const bgstyle = Selected ? 'bg-gradient-to-t from-[rgb(62,63,127)] to-[rgb(87,193,118)]' : 'bg-gradient-to-t from-[rgb(60,55,127)] to-[rgb(123,113,252)]';

    return (
      <button type='button'
              className={`
                ${Animate?'animate-translate':''}
                max-w-fit
                ${(Disabled===true)? ' bg-slate-600 ' : bgstyle}
                
                font-russoOne font-[100] text-[16px] text-primaryWhite
                shadow-md
                 ${(Disabled===true)?'': 'hover:brightness-[110%] hover:drop-shadow-[0px_0px_12px_#8d9afc] hover:scale-[.95]'}
                ${Style}
                `}
                
                onClick={(Disabled===true)?null: OnClick}
                title= {Tip}
      >
        {Caption} 
      </button>
    )
  }
  export default Button