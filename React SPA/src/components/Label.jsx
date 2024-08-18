
function Label({margin}) {
  return (
    
    <div className={margin? 'mt-[200px]' : 'mt-[10px]'}>
        <h1 className='
        flex justify-center
        text-primaryWhite font-russoOne font-[400] text-[23px] tracking-wide
        drop-shadow-[0_4px_4px_rgba(0,0,0,0.25)]
        '
        >Profile Match Bot</h1>
    </div>
  )
}

export default Label