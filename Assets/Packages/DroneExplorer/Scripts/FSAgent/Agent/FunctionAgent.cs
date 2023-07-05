using FSAgent.Agent.Component;
using FSAgent.Agent.Decorator;
using System;
using System.Threading.Tasks;
namespace FSAgent.Agent
{
    public class FunctionAgent<TargetType, RTargetType> where
        TargetType : BaseTargetType, new() where
        RTargetType : BaseTargetType, new()
    {
        private AgentBase<TargetType> _agent_adaptive;
        private AgentBase<RTargetType> _agent_reaction;

        // Default path of saved reactive compound action 
        private readonly string rDEFAULTCOMPOUNDPATH;

        // Default path of saved adaptive compound action 
        private readonly string aDEFAULTCOMPOUNDPATH;

        // Default path of saved reactive conditions
        private readonly string rDEFAULTCONDITIONSPATH;

        // Default path of saved adaptive conditions
        private readonly string aDEFAULTCONDITIONSPATH;



        public FunctionAgent(object driver)
        {
            rDEFAULTCOMPOUNDPATH = "/saved/r_d_comp_p";
            aDEFAULTCOMPOUNDPATH = "/saved/a_d_comp_p";
            rDEFAULTCONDITIONSPATH = "/saved/r_d_cond_p";
            aDEFAULTCONDITIONSPATH = "/saved/a_d_cond_p";



            _agent_adaptive = new AgentAdaptive<TargetType>();
            _agent_reaction = new AgentReaction<RTargetType>();
            ((AgentReaction<RTargetType>)_agent_reaction).
                PrepareToStart(driver);
            ((AgentAdaptive<TargetType>)_agent_adaptive).
                PrepareToStart(driver);
        }

        public void Import(string? a_compound_path = null,
            string? a_conditions_path = null,
            string? r_compound_path = null,
            string? r_conditions_path = null)
        {
            _agent_adaptive.Import(
                a_compound_path ?? aDEFAULTCOMPOUNDPATH,
                a_conditions_path ?? aDEFAULTCONDITIONSPATH
                );
            _agent_reaction.Import(
                r_compound_path ?? rDEFAULTCOMPOUNDPATH,
                r_conditions_path ?? rDEFAULTCONDITIONSPATH
                );
        }

        public void Save(string? a_compound_path = null,
            string? a_conditions_path = null,
            string? r_compound_path = null,
            string? r_conditions_path = null)
        {
            _agent_adaptive.Save(
                a_compound_path ?? aDEFAULTCOMPOUNDPATH,
                a_conditions_path ?? aDEFAULTCONDITIONSPATH
                );
            _agent_reaction.Save(
                r_compound_path ?? rDEFAULTCOMPOUNDPATH,
                r_conditions_path ?? rDEFAULTCONDITIONSPATH
                );
        }

        public void ClearPredicates()
        {
            _agent_adaptive.ClearPredicates();
            _agent_reaction.ClearPredicates();
        }

        public void UpdateAdaptive<NewMove>() where
            NewMove : AgentDecorator<TargetType>, new()
        {
            _agent_adaptive = new NewMove().
                Wrap(_agent_adaptive);
        }
        public void UpdateReaction<NewMove>() where
            NewMove : AgentDecorator<RTargetType>, new()
        {
            _agent_reaction = new NewMove().
                Wrap(_agent_reaction);
        }

        public void Run()
        {
            Execute(_agent_adaptive.RunBehavior);
        }

        public void CreateAdaptiveBehavior()
        {
            Execute(_agent_adaptive.CreateBehavior);
        }

        public void DropTarget()
        {
            _agent_adaptive.DropTarget();
            _agent_reaction.DropTarget();
        }

        public void CreateReactionBehavior()
        {
            _agent_reaction.RefreshGenerator();
            _agent_reaction.CreateBehavior();
        }

        public void PrintAdaptiveBehavior(string name)
        {
            _agent_adaptive.PrintBehavior(name);
        }

        private void Execute(Action action)
        {
            _agent_adaptive.RefreshGenerator();
            _agent_reaction.RefreshGenerator();
            while (true)
            {
                Task adaptive_agent_task = Task.Factory.StartNew(action);
                while (!((AgentReaction<RTargetType>)_agent_reaction).IsNeedReaction())
                {
                    if (adaptive_agent_task.IsCompleted)
                    {
                        return;
                    }
                }
                ((AgentAdaptive<TargetType>)_agent_adaptive).
                    CancelExecute();
                _agent_reaction.RunBehavior();
                adaptive_agent_task.Wait();
            }

        }

    }
    


}

