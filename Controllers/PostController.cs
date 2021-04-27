using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.API.Controllers
{
  // Implementacion del filtro en el scope del controlador
  // [ServiceFilter(typeof(ControllerFilterExample))]
  [Route("api/[controller]")]
  [ApiController]

  public class PostController : ControllerBase
  {

    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public PostController(IPostService postservice, IMapper mapper)
    {
      _postService = postservice;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
      var posts = await _postService.GetPosts();
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);
      return Ok(response);
    }

    [HttpGet("{id}")]
    // Implementacion del filtro en el scope de la acción
    // [ServiceFilter(typeof(ValidationFilter))]
    public async Task<IActionResult> GetPostById(int Id)
    {
      var post = await _postService.GetPostById(Id);
      var postDto = _mapper.Map<PostDto>(post); ;
      var response = new ApiResponse<PostDto>(postDto);
      return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PostDto postDto)
    {
      var post = _mapper.Map<Post>(postDto);
      await _postService.InserPost(post);

      postDto = _mapper.Map<PostDto>(post);
      var response = new ApiResponse<PostDto>(postDto);
      return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int Id, PostDto postDto)
    {
      var post = _mapper.Map<Post>(postDto);
      post.Id = Id;
      var result = await _postService.UpdatePost(post);
      var response = new ApiResponse<bool>(result);
      return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int Id)
    {
      var result = await _postService.DeletePost(Id);
      var response = new ApiResponse<bool>(result);
      return Ok(response);
    }

  }
}