import {useState, useEffect} from 'react'
import Label from './Label';
import Button from './Button';
import Header from './Header';
import Stats from './Stats';
import Form from './Form';

function Container() {
    const [appState, setAppState] = useState(null);
    // const [forms, setForms] = useState(null);
    const [formId, setFormId] = useState(0);

    useEffect(()=>{
        setAppState('started');
    },[]);

    const selectForm = (id) => {
        setFormId(id);
        setAppState('form');
    }
    

    const renderContent = () => {
        const btnStyle = 'rounded-[22px] py-5 px-9 my-[12px] mx-[12px]';
        switch (appState){
            case 'started':
                return (
                    <>
                        <Header IsImage={true}/>
                        <Label margin={true}/>
                        <Button Caption={"Start"} Style={`mt-[50px] mx-auto min-w-[40%] ${btnStyle}`} OnClick={()=>setAppState('stats')}/>
                    </>
                )
            case 'loading':
            return (
                <>
                    <Header IsImage={false}/>
                    <h1>LOADING.......</h1>
                </>
            )
            case 'stats':
                return(
                    <>
                    <Header IsImage={false}/>
                    <Stats selectForm={selectForm}/>
                    {/* <Button Caption={"BACK"} Style={`mt-[50px] mx-auto min-w-[40%] ${btnStyle}`} OnClick={()=>setAppState('started')}/> */}
                    </>
                )
            case 'form':
                return(
                    <>
                    <Header IsImage={false}/>
                    <Form formId={formId} onEnd={()=>setAppState('stats')}/>
                    </>
                ) 
        }
    }
  return (
    <div className="
    bg-gradient-to-t from-primaryBlack to-purple
    border-[3px] border-purpleBorder
    shadow-md shadow-slate-50
    flex flex-col justify-center w-full md:w-[600px]
    min-h-[100dvh]
    rounded-[40px] box-border
    m-auto relative">
        
        {renderContent()}
    </div>
  )
}

export default Container