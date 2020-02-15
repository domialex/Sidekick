using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Windows.LeagueOverlay.Betrayal.Controls
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Agent : UserControl
    {
        public Agent()
        {
            InitializeComponent();
        }

        public string AgentName { get; set; }

        public RewardValue AgentValue { get; set; }

        public string TransportationReward { get; set; }

        public RewardValue TransportationValue { get; set; }

        public string FortificationReward { get; set; }

        public RewardValue FortificationValue { get; set; }

        public string ResearchReward { get; set; }

        public RewardValue ResearchValue { get; set; }

        public string InterventionReward { get; set; }

        public RewardValue InterventionValue { get; set; }
    }
}
