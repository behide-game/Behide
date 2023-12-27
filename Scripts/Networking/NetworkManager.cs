namespace Behide.Networking;

using Godot;
using Behide.OnlineServices;
using System.Threading.Tasks;
using System.Linq;

public partial class NetworkManager : Node3D
{
    private Signaling signaling = null!;
    private WebRtcMultiplayerPeer multiplayer = new();

    public override void _EnterTree()
    {
        signaling = GetNode<Signaling>("/root/WebRtcSignaling");
    }

    public async Task<Result<RoomId>> StartHost()
    {
        multiplayer.CreateMesh(1);
        Multiplayer.MultiplayerPeer = multiplayer;

        // Handle new connection
        signaling.OfferIdCreationRequested += async askingPeerId =>
        {
            var peer = new OfferPeerConnection(signaling);
            multiplayer.AddPeer(peer.GetPeerConnection(), askingPeerId);

            return await peer.CreateOffer();
        };

        return await signaling.CreateRoom();
    }

    public async Task StartClient(RoomId roomId)
    {
        // Retrieve offer and peer id
        var joinRoomInfo = await signaling.JoinRoom(roomId) switch
        {
            Result<JoinRoomRes>.Ok res => res.Value,
            Result<JoinRoomRes>.Error error => throw new System.Exception("Failed to retrieve offer ids: " + error.Failure),
            _ => throw new System.NotImplementedException()
        };

        multiplayer.CreateMesh(joinRoomInfo.PeerId);
        Multiplayer.MultiplayerPeer = multiplayer;

        // Handle following players connection
        signaling.OfferIdCreationRequested += async askingPeerId =>
        {
            var peer = new OfferPeerConnection(signaling);
            multiplayer.AddPeer(peer.GetPeerConnection(), askingPeerId);

            return await peer.CreateOffer();
        };

        // Connect to other players
        var tasks = joinRoomInfo.PlayersConnectionInfo.Select(async connInfo =>
        {
            GameManager.Ui.Log($"Connecting to {connInfo.PeerId}");
            var peer = new AnswerPeerConnection(signaling, connInfo.OfferId);
            multiplayer.AddPeer(peer.GetPeerConnection(), connInfo.PeerId);

            await peer.Connect();
        });

        await Task.WhenAll(tasks);
    }
}
