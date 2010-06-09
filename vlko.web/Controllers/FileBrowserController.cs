﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vlko.core.Base;
using vlko.core.Models.Action;
using vlko.core.Models.Action.ViewModel;
using vlko.model.IoC;
using vlko.web.ViewModel.FileBrowser;

namespace vlko.web.Controllers
{
    [Authorize]
    public class FileBrowserController : BaseController
    {
        /// <summary>
        /// URL: FileBrowser/Index
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Index()
        {
            var files = IoC.Resolve<IFileBrowserAction>().GetAllUserFileInfos(UserInfo.Name);
            return ViewWithAjax(new FileBrowserViewModel
                            {
                                UserFiles = files
                            });
        }

        /// <summary>
        /// URL: FileBrowser/Delete
        /// </summary>
        /// <param name="ident">The ident.</param>
        /// <returns>Action result.</returns>
        public ActionResult Delete(string ident)
        {
            return ViewWithAjax(
                IoC.Resolve<IFileBrowserAction>()
                    .GetFileInfo(UserInfo.Name, ident));
        }

        /// <summary>
        /// URL: FileBrowser/Delete
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult Delete(FileViewModel model)
        {
            if (!IoC.Resolve<IFileBrowserAction>()
                .DeleteFile(UserInfo.Name, model.Ident))
            {
                ModelState.AddModelError(string.Empty, vlko.core.ModelResources.FileDeleteFailedError);
                return ViewWithAjax(model);
            }
            return RedirectToActionWithAjax("Index");   
        }

        /// <summary>
        /// URL: FileBrowser/Upload
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var fileBrowserActions = IoC.Resolve<IFileBrowserAction>();

            var model = new FileBrowserViewModel
                            {
                                Ident = Request.Form["Ident"]
                            };

            // test content length
            if (file.ContentLength > FileBrowserViewModel.MaxFileSize)
            {
                ModelState.AddModelError("Ident", string.Format(vlko.core.ModelResources.FileTooBigError, FileBrowserViewModel.MaxFileSize));
            }
            else
            {
                // test if ident specified
                if (string.IsNullOrWhiteSpace(model.Ident))
                {
                    ModelState.AddModelError("Ident", vlko.core.ModelResources.FileIdentRequireError);
                }
                else
                {
                    // get new ident based on user ident and uploaded file extension
                    string fileIdent = model.Ident + Path.GetExtension(file.FileName);

                    // check if file exists
                    var existingItem = fileBrowserActions.GetFileInfo(UserInfo.Name, fileIdent);
                    if (existingItem != null)
                    {
                        ModelState.AddModelError("Ident", string.Format(vlko.core.ModelResources.FileIdentExistsError, fileIdent));
                    }
                    else
                    {
                        // Save file
                        if (!fileBrowserActions.SaveFile(UserInfo.Name, fileIdent, file.InputStream))
                        {
                            ModelState.AddModelError("", vlko.core.ModelResources.FileUploadFailedError);
                        }
                        else
                        {
                            model.Ident = null;
                        }
                    }
                }
            }

            // load user files
            model.UserFiles = fileBrowserActions.GetAllUserFileInfos(UserInfo.Name);

            // return normal view, for ajax we have iframe technique so normal result is needed
            return View("Index", model);
        }

    }
}
