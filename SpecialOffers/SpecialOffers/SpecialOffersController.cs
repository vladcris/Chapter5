using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpecialOffers.Events;

namespace SpecialOffers.SpecialOffers;

[ApiController]
[Route("/offers")]
public class SpecialOffersController : ControllerBase
{
    private readonly IEventStore _eventStore;
    private readonly static IDictionary<int, Offer> Offers = new Dictionary<int, Offer>();

    public SpecialOffersController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Offer> GetOffer(int id)
    {
        return Offers.ContainsKey(id)
            ? Ok(Offers[id])
            : NotFound();
    }

    [HttpPost("")]
    public ActionResult<Offer> CreateOffer([FromBody] Offer offer)
    {
        if(offer == null)
        {
            return BadRequest();
        }

        var newOffer = NewOffer(offer);
        return Created(new Uri($"/offers/{newOffer.Id}", UriKind.Relative), newOffer);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Offer> UpdateOffer(int id, [FromBody] Offer offer)
    {
        var offerWithId = offer with {Id = id};
        _eventStore.RaiseEvent("SpecialOfferUpdated", new{OldOffer = Offers[id], NewOffer = offerWithId});
        return Offers[id] = offerWithId;
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteOffer(int id)
    {
        _eventStore.RaiseEvent("SpecialOfferRemoved", new{Offer = Offers[id]});
        Offers.Remove(id);
        return Ok();
    }

    private Offer NewOffer(Offer offer)
    {
        var id = Offers.Count;
        var newOffer = offer with {Id = id};
        _eventStore.RaiseEvent("SpecialOfferCreated", newOffer);
        return Offers[id] = newOffer;
    }


    public record Offer(string Description, int Id);

}
