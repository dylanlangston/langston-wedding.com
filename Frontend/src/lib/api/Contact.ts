/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { ContactRequest } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Contact<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * @description Accepts a JSON request with contact details and validates the email.
   *
   * @tags Contact
   * @name Contact
   * @summary Processes a contact request
   * @request POST:/Contact
   */
  contact = (data: ContactRequest, params: RequestParams = {}) =>
    this.request<string, string>({
      path: `/Contact`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
